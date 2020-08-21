using System;

namespace PracticaAvanzado1
{
    //declarar delegado
    public delegate float CalcularTotal(float subtotal);
    public delegate void CalcularTotalRef(ref float subtotal); //para multicast ref                                                               //para anonimos
    public delegate void ImprimirMensaje(string msj); //para anonimos

    class VueloNacional
    {
        float Iva //propiedad
        {
            get
            {
                if (Redondo)
                    return 0.16f;
                return 0.04f;
            }
        }
        float Tua
        {
            get
            {
                if (Redondo)
                    return 220f * 2;
                return 220f;
            }
        }
        public bool Redondo { get; set; }
        public float CalcularMontoTotal(float monto)
        {
            return monto + (monto * Iva) + Tua;
        }
    }
    class VueloInternacional
    {
        float Iva
        {
            get
            {
                if (Redondo)
                    return 0.16f;
                return 0.04f;
            }
        }
        float Tua
        {
            get
            {
                if (Redondo)
                    return 360f * 2;
                return 360f;
            }
        }
        float ImpuestoFederalSeguridad
        {
            get
            {
                return 190.75f;
            }
        }
        public bool Redondo { get; set; }
        public int Destino { get; set; }
        public float CalcularMontoTotal(float monto)
        {
            float total = monto + (monto * Iva) + Tua;
            if (Destino == 559)
                return total + ImpuestoFederalSeguridad;
            return total;
        }
        //para multicast
        public void CalcularTotalConImpuestos(ref float monto)
        {
            float total = monto + (monto * Iva) + Tua;
            if (Destino == 559)
                total += ImpuestoFederalSeguridad;
            monto = total;
        }
        
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            #region Delegados
            VueloNacional vueloNac = new VueloNacional
            {
                Redondo = true,
            };
            CalcularTotal total = new CalcularTotal(vueloNac.CalcularMontoTotal); //instanciar
            //CalcularTotal total = vueloNac.CalcularTotal; //instanciar modo corto

            float precio = 5500f;
            Console.WriteLine("Costo vuelo nacional redondo: ${0}", total(precio)); //invocar

            //vuelo internacional
            VueloInternacional vueloInter = new VueloInternacional
            {
                Redondo = false,
                Destino = 559
            };
            total = vueloInter.CalcularMontoTotal; //instanciar modo corto
            float precioVueloInter = 9800f;
            float totalVuelInter = total(precioVueloInter);//invocar
            Console.WriteLine("Costo vuelo sencillo internacional a EU: ${0}", totalVuelInter); 
            #endregion

            #region delegados como parámetro
            float totalAdultoMayor = CalcularConDescuentoAdultoMayor(precioVueloInter, vueloInter.CalcularMontoTotal);
            //o directamente usando el delegado que habiamos asignado a total
            //float totalAdultoMayor = CalcularConDescuentoAdultoMayor(totalVuelInter, total);
            Console.WriteLine("Costo vuelo sencillo internacional a EU, adulto mayor: ${0}", totalAdultoMayor);
            #endregion

            #region delegados multicast
            CalcularTotal t = vueloInter.CalcularMontoTotal; // instancia del delegado
            t = t + CalcularTotalSeguro; //agregar otro metodo target
            //o en la forma corta
            // t += CalcularTotalSeguro;
            //Console.WriteLine("Costo vuelo sencillo internacional a EU, con seguro: ${0}", t(precioVueloInter));

            //con ref
            CalcularTotalRef tr = vueloInter.CalcularTotalConImpuestos; //instancia del delegado
            tr += CalcularTotalConSeguroRef; //agregamos otro metodo target
            tr(ref precioVueloInter); //invocar delegado
            Console.WriteLine("Costo vuelo sencillo internacional a EU, con seguro: ${0}", precioVueloInter);
            #endregion

            #region delegados anónimos
            ImprimirMensaje im = delegate(string message) //anonimo
            {
                Console.WriteLine("Mensaje: {0}", message);
            };
            //invocar
            im("Delegados anónimos");
            #endregion
        }

        //Para delegados como parámetro
        static float CalcularConDescuentoAdultoMayor(float monto, CalcularTotal total)
        {
            float subtotal = total(monto);
            return subtotal - (0.35f * subtotal);
        }
        //para delegados multicast
        static float CalcularTotalSeguro(float total)
        {
            float porcentajeSeguro = 0.1f;
            return total * porcentajeSeguro;
        }
        //delegados multicast ref
        static void CalcularTotalConSeguroRef(ref float total)
        {
            float porcentajeSeguro = 0.1f;
            total += total * porcentajeSeguro;
        }
    }

    
}
