using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ODE FUNCTIONS ===================================================
namespace ODE{
    public delegate void ODEFunction<T>(ref List<T> du, T t, List<T> u, List<T> p);
    public delegate void CollisionHandler(ref List<double> u, List<double> du, double dt);

    public struct ODEProblem<T>{
        // f(du, t, u, p) => du: derivada de u respecto a t
        public ODEFunction<T> Function; // Funtion to solve
        public List<T> U0;              // Initial solution
        public Tuple<T, T> TSpan;       // Integration interval
        public List<T> Parameters;      // Function parameters
        
        public ODEProblem(ODEFunction<T> function, List<T> u0, Tuple<T, T> tspan, List<T> p){
            Function     = function;
            U0          = u0;
            TSpan       = tspan;
            Parameters  = p;
        }

    }

    public struct ODEProblemSolution<T>{
        public List<T> TT;              // List of timestamps
        public List<List<T>> UU;        // List of solutions
        public ODEProblem<T> Problem;   // EDO problem

        public ODEProblemSolution(List<T> tt, List<List<T>> uu, ODEProblem<T> problem){
            TT      = tt;
            UU      = uu;
            Problem = problem;
        }
    }

    public static class ODESolver{
        public static ODEProblemSolution<double> Euler(ODEProblem<double> problem, double dt, CollisionHandler collisionHandler = null) {
            double t0 = problem.TSpan.Item1;
            double tf = problem.TSpan.Item2;
            var u = new List<double>(problem.U0); // Current state
            var t = t0;

            var TT = new List<double> { t };
            var UU = new List<List<double>> { new List<double>(u) };

            var du = new List<double>(new double[u.Count]);

            while (t < tf){
                problem.Function(ref du, t, u, problem.Parameters);

                // Euler integration: u = u + dt * du
                for (int i = 0; i < u.Count; i++)
                    u[i] += dt * du[i];

                // Collision detection
                collisionHandler?.Invoke(ref u, du, dt);

                t += dt;
                TT.Add(t);
                UU.Add(new List<double>(u));
            }

            return new ODEProblemSolution<double> { TT = TT, UU = UU, Problem = problem };
        }
    }

}

// =================================================================



[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class BallTrajectoryPredictor : MonoBehaviour{
    public double simulationTime = 3f;

    public float substepThreshold = 0.5f; // Si la distancia al colisionar es < este valor, se subdivide

    public LineRenderer lineRenderer = null;

    private Rigidbody ball;
    private SphereCollider ballCollider;

    // Ball ODE Function
    public static void GravityAndDragBall(ref List<double> du, double t, List<double> u, List<double> p){
        // u = [x, y, z, vx, vy, vz]
        // p = [gx, gy, gz, dragCoef]

        double gx = p[0];
        double gy = p[1];
        double gz = p[2];
        double k = p[3];

        // Assert u and du has the same dimensions
        if (du.Count != u.Count)
            throw new InvalidOperationException("u and du dimensions mismatch!!");

           
        du[0] = u[3];   // dx/dt = vx
        du[1] = u[4];   // dy/dt = vy
        du[2] = u[5];   // dz/dt = vz
        du[3] = gx - k * u[3];  // dvx/dt = gx - k * vx
        du[4] = gy - k * u[4];  // dvy/dt = gy - k * vy
        du[5] = gz - k * u[5];  // dvz/dt = gz - k * vz
    }

    // Ball Collision Handler
    public static void BallCollisionHandler(ref List<double> u, List<double> du, double dt, float ballRadius, PhysicMaterial ballMaterial){
        Vector3 position = new Vector3((float)u[0], (float)u[1], (float)u[2]);
        Vector3 velocity = new Vector3((float)u[3], (float)u[4], (float)u[5]);
        Vector3 step = velocity * (float)dt;

        if (Physics.SphereCast(position, ballRadius, step.normalized, out RaycastHit hit, step.magnitude)){
            // Obtener el material de física del objeto con el que colisiona
            var colliderMaterial = hit.collider.material;
            if (colliderMaterial != null){
                // Parameters of the ball physics material
                float ballRestitution       = ballMaterial.bounciness;
                float ballDynamicFriction   = ballMaterial.dynamicFriction;
                float ballStaticFriction    = ballMaterial.staticFriction;

                // Parameters of the collider's object
                float objectRestitution     = colliderMaterial.bounciness;
                float objectDynamicFriction = colliderMaterial.dynamicFriction;
                float objectStaticFriction  = colliderMaterial.staticFriction;

                // Compute restitution and friction
                float restitution = Mathf.Max(ballRestitution, objectRestitution);      // "Maximum" as the ballMaterial has that property
                float friction = Mathf.Min(ballDynamicFriction, objectDynamicFriction); // "Minimum" as the ballMaterial has that property

                // Compute the new velocity with the collision norm and apply restitution and friction
                Vector3 correctedPos = hit.point + hit.normal * ballRadius;
                Vector3 newVelocity = Vector3.Reflect(velocity, hit.normal) * restitution;

                if (newVelocity.magnitude < 0.1f)
                    friction = Mathf.Max(ballStaticFriction, objectStaticFriction);
                newVelocity *= (1.0f - friction);

                // Update position and velocity
                u[0] = correctedPos.x;
                u[1] = correctedPos.y;
                u[2] = correctedPos.z;
                u[3] = newVelocity.x;
                u[4] = newVelocity.y;
                u[5] = newVelocity.z;
            }
        }
    }
   

    // Start is called before the first frame update
    void Start(){
        ball            = GetComponent<Rigidbody>();
        ballCollider    = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update(){
        
        // Initial conditions and parameters
        Vector3 i_p = ball.position;
        Vector3 i_v = ball.velocity;
        List<double> u0 = new List<double> { i_p.x, i_p.y, i_p.z, i_v.x, i_v.y, i_v.z };
        List<double> p = new List<double> { Physics.gravity.x, Physics.gravity.y, Physics.gravity.z, ball.drag};

        var ball_ode = new ODE.ODEProblem<double>(
            function: GravityAndDragBall,
            u0: u0, // [x, y, z, vx, vy, vz]
            tspan: Tuple.Create(0.0, simulationTime),
            p: p
        );

        // Solver
        var solution = ODE.ODESolver.Euler(
            ball_ode, 
            Time.fixedDeltaTime, 
            collisionHandler: (ref List<double> u, List<double> du, double dt) => {
                BallCollisionHandler(ref u, du, dt, 0.05f, ballCollider.material);
            });
        
        // Render
        if (lineRenderer is not null){
            Vector3[] positions = solution.UU.Select(u => new Vector3((float)u[0], (float)u[1], (float)u[2])).ToArray();
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }

        
    }
}


