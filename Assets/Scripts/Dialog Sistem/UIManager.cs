using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool ejeEnUso = false;
    public GameObject[] flechas;
    private int index;
    void Update()
    {

        if (DialogManager.instance._isChossing)
        {
            float valorVertical = Input.GetAxisRaw("Vertical");

            if (valorVertical != 0)
            {

                if (!ejeEnUso)
                {
                    if (valorVertical > 0)
                    {

                        EjecutarAccionArriba();
                    }
                    else if (valorVertical < 0)
                    {

                        EjecutarAccionAbajo();
                    }


                    ejeEnUso = true;
                }
            }
            else
            {

                ejeEnUso = false;
            }

            if (Input.GetButtonDown("Submit"))
            {
                if (index != flechas.Length - 1) DialogManager.instance.MakeChoice(index);
            }
        }
    }

    void EjecutarAccionArriba() {
        index--;
        if(index < 0) index = flechas.Length-1;
        MostrarFlechas();

       
    }
    void EjecutarAccionAbajo() { 
        index++;
        if (index > flechas.Length - 1) index = 0;
        MostrarFlechas();

    }

    void MostrarFlechas()
    {
        for (int i = 0; i < flechas.Length; i++)
        {
            if (i != index) flechas[i].SetActive(false);
            else flechas[i].SetActive(true);
        }
    }
}
