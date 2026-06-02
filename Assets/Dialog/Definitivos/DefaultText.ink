VAR count = 0

Hola persona

Me gustaría saber como me tengo que referir a ti, así que te preguntaré que eres.
->Bucle

===Bucle===
#mostrarPanel
¿Eres un chico o una chica?

 + [Chico] -> Chico
 + [Chica] -> Chica
 + [?] 
 ~ count++ 
 -> Nada


===Chico===
Así que eres un chico
Perfecto, es lo que se suele esperar de alguien que juega a videojuegos
Pues comenzaremos de una vez, aunque ya te aviso de que no vas a poder llegar muy lejos
#siguientePantalla

->DONE

===Chica===
Así que eres una chica
No sé si quieres que te haga un tutorial especial, ya que no creo que sepas jugar, o siquiera vayas a entender bien como se juega a esto
Bueno no tenemos tanto tiempo, te tocará adaptarte
Pues comenzaremos de una vez, aunque ya te aviso de que no vas a poder llegar muy lejos
#siguientePantalla

->DONE

===Nada===
{ count == 1:
    <>
    ¿Por qué pulsas esa opción? No ves que no hay nada
    Te tendré que volver a hacer la pregunta, así que contesta claro esta vez<>
}

{ count == 2:
    <>
    ...
    ¿Qué se supone que estás intentando hacer?
    Podrías dejar de hacerme perder el tiempo y pulsar una opción que sea correcta
}

{ count == 3:
    <>
    Empiezo a pensar que no sabes leer
    Podrías hacerme el favor de pulsar una opción de las que te ofrezco
}

{ count == 4:
    <>
    Realmente ni me importa lo que seas. Preguntarte tu género solo es una formalidad
    ¿Sabes que estás perdiendo el tiempo, verdad?

}

{ count == 5:
    <>    
    Puedo estar así todo el día .............
    incluso podría decidir yo tu genero (si quien me programo no fuera tan estupido para no haberme dotado de poder hacerlo)
}

{ count == 6:
    <>
    Desisto de contestarte
    ...
}

{ count > 6:
    <>
    ...
}


-> Bucle

    -> END
