-> main

=== main ===
Dime como estás
    + [Si]
        -> seleccion("Acepto la misión")
    + [No]
        -> seleccion("No quiero saber nada")
    + [Me rindo]
        -> seleccion("No puedo más")
        
    === seleccion(selec) ===
    
    {selec}
    -> END
