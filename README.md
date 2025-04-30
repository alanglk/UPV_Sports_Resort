
# UPV Sports Resort Project

## Forma de trabajar con Git
Este proyecto es desarrollado por un grupo de 8 personas, por lo que es muy importante tener una gestión adecuada de las versiones. Para ello se va a hacer uso de la metodología _GitFlow_. Esta metodología consiste en dividir el proyecto en desarrollos independientes de features que se van incorporando en un proyecto común final de manera progresiva. Para realizar esto se definen tres tipos de ramas principales:

- Rama __main__: Es la rama del proyecto final. No se puede trabajar directamente sobre esta rama. El único caso en el que se puede hacer un comit en este workspace es a la hora de publicar una versión "final" del proyecto.
- Rama __develop__: Es la rama en la que se realizan las integraciones de las features. Supongamos que se han desarrollado dos minijuegos del proyecto y se quiere crear una version intermedia que los incluya. Esto se realizará en esta rama de forma que se hace un merge de minijuego y luego del otro (resolviendo los correspondientes conflictos y demás).
- Rama de __feature__: Cada desarrollo independiente se realizará en su rama correspondiente. De esta manera no habrán problemas de conflictos durante el desarrollo de features. 

> [!WARNING] 
> IMPORTANTE NO HACER COMMITS O TRABAJAR DIRECTAMENTE SOBRE LA RAMA MAIN O DEVELOP QUE SE PUEDE LIAR PARDA!!!!

![alt text](./Documentation/images/gitflow_graph.png "Title")

La idea es que por cada minijuego/escena (el lobby también cuenta) haya al menos una rama y, al estar por parejas en cada desarrollo, es aconsejable que cada persona tenga su rama independiente.

Pongámonos en el caso de que Iosu está desarrollando el bateo para el minijuego de beisbol en su rama independiente `beisbol_iosu`, mientras que Ane ha creado el escenario y los materiales en la rama `beisbol_ane`. El proceso para juntar los desarrollos sería hacer un _merge_ de `beisbol_iosu` a la rama del minijuego `beisbol`, luego hacer _merge_ de la rama `beisbol_ane` y, finalmente si el minijuego ya se ha terminado y se quiere integrar con el resto del projecto, habría que hacer un merge de la rama `beisbol` a la rama `develop`.

Los comandos básicos para realizar esto sería (en el caso de Iosu):
```bash
# Creamos la rama de desarrollo de Iosu desde la rama de la Feature beisbol
git pull # Actualizar el repositorio local con los cambios en la nube

git checkout beisbol # Nos movemos a la rama beisbol
git branch beisbol_iosu # Creamos la rama beisbol_iosu
git checkout beisbol_iosu # Nos movemos a la rama beisbol_iosu

# ... DESARROLLO EN LA RAMA DE IOSU
git add <cambios> # Añadimos los cambios
git commit -am "Mensaje de commit de los cambios (que sea representativo)"

# ... SE HA TERMINADO DE DESARROLLAR LA FEATURE DE IOSU
git checkout beisbol # Nos movemos a beisbol
git merge beisbol_iosu # Traemos los cambios de beisbol_iosu a la rama beisbol
```


