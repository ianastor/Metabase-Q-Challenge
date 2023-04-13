# Metabase-Q-Challenge


Este es el challenge que se me solicito, de parte de Metabase Q, cumple con todas las consignas y debería funcionar sin problema alguno.

En cuanto a que me gustaría mejorar a futuro, lo primero seria realizar ambas consultas a la vez como forma de ahorrar tiempo, segundo creo que sería mejor usar otra api distinta a la de geolocation, ya que la misma trae varios errores, debido a que se trata de una api gratuita que fue usada en este momento para realizar el challenge, me gustaría agregar optimizaciones de código varias, asimismo campos extra a agregar al CSV


# Bonus! Challenge 2: Conceptos

Mis respuestas al bonus:

 ## Explicar las diferencias entre cifrado y compresión y determinar si es más conveniente cifrar primero y luego comprimir, o al revés, y por qué.
 
1- El cifrado y la compresión son dos técnicas usadas en el procesamiento de datos, con propósitos diferentes. Ambos implican la modificación de la información original. El cifrado consiste en utilizar algoritmos y claves para transformar los datos en un formato ilegible o incomprensible denominado texto cifrado. Esto se hace para proteger la confidencialidad e integridad de la información. Esto se hace para que solo los usuarios autorizados con la clave correcta pueden descifrar los datos y acceder al contenido original. El cifrado se usa comúnmente para proteger la confidencialidad de la información confidencial, como información personal, información financiera y comunicaciones confidenciales.

La compresión, por otro lado, reduce el tamaño de los datos para usar menos espacio de almacenamiento o para transmitir datos de manera más eficiente a través de la red. La compresión se basa en algoritmos que eliminan redundancias o patrones repetitivos en los datos, lo que reduce la cantidad de bits necesarios para representar la información. La compresión se usa comúnmente para ahorrar espacio en disco y acelerar la transferencia de datos a través de redes con ancho de banda limitado.
Si es más conveniente cifrar primero y luego comprimir, o viceversa, depende del contexto del sistema y de los requisitos del mismo, pero dentro de lo que podríamos nombrar


Si ciframos primero, podemos obtener mayor seguridad, Al cifrar primero los datos, se protege su confidencialidad e integridad antes de la compresión, lo que garantiza que los datos comprimidos también estén protegidos o por ejemplo mayor flexibilidad, cuando se comprimen datos ya cifrados, se pueden usar algoritmos de compresión mas eficientes, porque los datos ya cifrados son mas repetitivos y tienen menos redundancias.

Todo depende de las configuraciones, políticas y reglas de seguridad.

Herramientas como veracrypt, permiten tener ambas soluciones 

## Suponiendo que la red de un cliente ha sido comprometida y se han detectado 10 equipos infectados con el troyano Emotet, el principal vector de entrada de este malware es el phishing, ¿cómo se le explicaría al cliente lo que ha sucedido?


2- Creo que lo primero seria explicarle al cliente de que se trata el phishing, para eso recaigo en la definición del mismo: La palabra phishing quiere decir suplantación de identidad.
Es una técnica de ingeniería social que usan los ciberdelincuentes para obtener información confidencial de los usuarios de forma fraudulenta y así apropiarse de la identidad de esas personas.
Los ciberdelincuentes envían correos electrónicos falsos como anzuelo para “pescar” contraseñas y datos personales valiosos. (Argentina.gob.ar)

Le explicaría que los usuarios de la empresa fueron engañados usando estas étnicas, haciendo que descarguen y ejecuten el malware en los sistemas.

Emonet es conocido por robar información sensible y propagarse en la red, permite el acceso remoto y no autorizado.
Le enviaría un correo o les daría un mensaje con los próximos pasos,

1-	Aislar y desconectar los dispositivos infectados

2-	Escanear y limpiar todos los equipos y continuar la búsqueda del malware en la red con una herramienta SIEM en lo posible

3-	Cambiar todas las contraseñas y claves de administrador, armar una lista de dispositivos con posibles cuentas 
comprometidas.

4-	Miraría los ids/ips en búsqueda de conexiones extrañas, asimismo, lo mejor seria tener una buena política de backups y tener los sistemas actualizados

5-	Como último paso lo mejor seria capacitar a los usuarios para poder evitar incidentes a futuro 


## ¿Cuáles son las diferencias entre encodeado, hashing y cifrado?

3- 1-	El encodeado es un proceso de conversión de los datos de un formato a otro, generalmente se usa para asegurarse que los datos son legibles y usables en distintos sistemas o aplicaciones

2-	El hashing es un proceso de transformación de los datos en una cadena de caracteres alfanuméricos de longitud fija, es un proceso irreversible. El hash se calcula a partir de los datos originales haciendo uso de un algoritmito matemático, cualquier cambio en los datos originales, resulta en un cambio completo del hash. Se hace mucho de los hashes para verificar la integridad del mismo

3-	El cifrado es un proceso de transformación de los datos en un proceso ilegible, conocido como texto cifrado, mediante el uso de algoritmo o claves. El cifrado se usa para proteger la confidencialidad de la información, debido a que solo las personas con las claves correspondiente pueden acceder a los datos o su contenido original. A diferencia del hashing, este se puede revertir. Un ejemplo muy común de esto, es el rot-13 o código cesar 


