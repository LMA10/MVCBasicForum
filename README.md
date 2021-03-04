![image](https://user-images.githubusercontent.com/34746719/110036402-b5a4d680-7d1b-11eb-9411-775812350275.png)
![image](https://user-images.githubusercontent.com/34746719/110036433-c1909880-7d1b-11eb-8afd-79e4638c7882.png)
![image](https://user-images.githubusercontent.com/34746719/110036362-a58cf700-7d1b-11eb-8563-4ae4fc905d45.png)
![image](https://user-images.githubusercontent.com/34746719/110036464-cfdeb480-7d1b-11eb-8c94-dbfb42c2ce49.png)

# Foro 📖
Sistema que permite la administración general de un Foro (de cara a los administradores): Usuarios, Miembros, Administradores, Pregunta, Respuesta, MeGusta, etc., como así también, dar la posibilidad a los usuarios de navegar el foro y realizar preguntas y/o respuestas. 
utilizando ASP.NET MVC Core versión 3.1.

<hr />

## Proceso de ejecución en alto nivel ☑️
 - Generacion de un nuevo proyecto en [visual studio]
 - agregado de todos los modelos dentro de la carpeta Models (cada uno en un archivo separado).
 - Especificacion de todas las restricciones y validaciones solicitadas a cada una de las entidades.
 - Creacion de todas las relaciones entre las entidades
 - Creacion de una carpeta Data con el file correspondiente al contexto de datos. 
 - Creacion del DbContext utilizando base de datos en memoria (con fines de testing inicial).
 - Agregado de los DbSet correspondientes para cada una de las entidades en el DbContext.
 - Creacion del Scaffolding para permitir los CRUD de las entidades.
 - Aplicacion de las adecuaciones y validaciones necesarias en los controladores.  
 - Realizacion del sistema de login con al menos los roles de Usuario y Administrador
 
 NOTA 1: Un administrador puede realizar todas tareas que impliquen interacción del lado del negocio (ABM "Alta-Baja-Modificación" de las entidades del sistema y configuraciones en caso de ser necesarias).
 NOTA 2: El <Usuario Cliente> sólo puede tomar acción en el sistema, en base al rol que tiene.

<hr />

## Entidades 📄
- Miembro
- Administrador
- Categoria
- Entrada
- Pregunta
- Respuesta
- Like (MeGusta)

**Administrador**
```
- Nombre
- Apellido
- Email
- FechaAlta
- Password
```

**Miembro**
```
- Nombre
- Apellido
- Email
- Telefono
- FechaAlta
- Password
- Entradas
- Preguntas
- Respuestas
- RespuestasQueMeGustan
```

**Categoria**
```
- Nombre
- Entradas
```

**Entrada**
```
- Titulo
- Fecha
- Categoria
- Miembro
- Preguntas
- Privada
- MiembrosHabilitados
```

**Pregunta**
```
- Descripcion
- Entrada
- Respuestas
- Miembro
- Fecha
- Activa
```

**Respuesta**
```
- Descripcion
- Pregunta
- Miembro
- Fecha
- Reacciones (colección de Likes, "MeGusta")
```

**Like**
```
- Fecha
- MeGusta
- Respuesta
- Miembro
```

<hr />

## Caracteristicas y Funcionalidades ⌨️

**Administrador**
- Un administrador, sólo puede: crear nuevas categorías.
- Los administradores del Foro deben ser agregados exclusivamente por otro Administrador.
- Al momento del alta del Administrador se le define un username y password.
- También se le asigna a estas cuentas el rol de Administrador automáticamente.

**Miembro**
- Puede auto registrarse.
- La autoregistración desde el sitio, es exclusiva para los usuarios de tipo "miembro", por lo cual, se le asignará dicho rol automáticamente.
- Los miembros pueden navegar por el foro.
- Pueden crear Entradas y automaticamente generará una pregunta.
- Pueden desactivar una pregunta en cualquier momento. Si una pregunta está inactiva no se dejará de ver, simplemente impedirá que carguen nuevas respuestas otros miembros.
- No se puede cargar una respuesta de una pregunta del mismo miembro. Esta acción debe estar deshabilitada.
- Puede crear nuevas categorías.
- Antes de crearla, se le propondrá un listado de categorias ya existentes en orden alfabético.
- Se puede poner Like (Me gusta), Dislike (No me gusta) o ninguna acción (Quitar la reacción) a cualquier respuesta que no sea propia (que no sea de su propia autoría).

**Reaccion**
- La reacción a una respuesta será validándola con las 3 posibilidades (me gusta, no me gusta o nada).
- Al quitar la reacción, no se desea guardar registro previo de la misma.
    - Un miembro, solo puede quitar las reacciones que uno mismo ha cargado.

**Entrada**
- Al generar una entrada por un miembro, quedarán los datos básicos asignados, como ser fecha, el miembro que la creó, etc.
    - La categoria puede ser una existente o una nueva que quiera crear en el momento.
- La entrada creará junto con ésta la primer pregunta que también será el mismo miembro el autor.
    - Las entradas listarán las preguntas en orden cronológico ascendente.
    - Estas preguntas mostrarán al costado la cantidad de respuestas que recibieron.
- La entrada puede ser privada, en tal caso, se listará en el foro, con su título, pero sólo miembros habilitados, podrán acceder a las preguntas y respuestas para interactuar.
    - El creador de la entrada no necesita ser habilitado explícitamente.
    - Los miembros no habilitados pueden solicitar que se los habilite.
    - Un miembro autor de la entrada podrá ver un listado de miembros que quieren ser habilitados y habilitarlos uno por uno.

**Pregunta**
- Mientras que una pregunta esté activa, otros miembros podrán dar respuestas a las preguntas.
- La entrada puede tener más preguntas del mismo miembro, como asi también, recibir más preguntas de otros miembros.
- Se visualizará las respuestas en orden cronológico ascendente, al acceder a cada pregunta.
    - La respuesta con más likes, se destacara visualmente. (Con un recuadro verde en este caso). 
    - La respuesta con más dislikes, se destacara visualmente. (Con un recuadro rojo en este caso).

**Respuesta**
- Las respuestas serán cargadas por miembros que no son los autores de las preguntas.
- Podrán recibir reacciones.

**Reacciones**
- Las reacciones, acerca de las respuestas, no pueden ser realizadas por los mismos autores de las respuestas. 


**Aplicación General**
- El foro, mostrará los encabezados en la home:
    - Un listado de las ultimas 5 entradas cargadas más recientemente. 
    - Un top 5 de Entradas con más preguntas y respuestas.
    - Un top 3 de los miembros con más entradas cargadas en el ultimo mes. 
- Se debe ofrece también una navegación por categorías. 
- Los miembros no pueden eliminar las entradas ni deshabilitarlas.
- Sólo los administradores pueden eliminar entradas con sus preguntas y respuestas funcionando como moderadores en caso de que éstas tengan contenido inapropiado.
- Los accesos a las funcionalidades y/o capacidades, estan basadas en los roles que tenga cada individuo.
