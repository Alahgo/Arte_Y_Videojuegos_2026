-> main

=== main ===
Dime como estás
    + [Bien]
        -> seleccion("Muy bien, gracias por preguntar")
    + [Regular]
        -> seleccion("Pisi Pasa")
    + [Mal]
        -> seleccion("Fatal, me quiero morir")
        
    === seleccion(selec) ===
    
    {selec}
    -> END
