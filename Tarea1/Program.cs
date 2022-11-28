// See https://aka.ms/new-console-template for more information

using System;

namespace  Programa
{
    class program{
        
        static void Main(string[] args){
            int [] billetes = new int[5] {500,200,100,50,20};
            int [] monedas = new int[3] {10,5,1};
            List<int> dinero = new List<int>();
            List<int> dinero2;
            Time.deltatime()
            bool check = false;
            
            while(check==false){
                Console.WriteLine("-----------------Banco CDIS -------------------");
                Console.WriteLine("1. Ingresar la cantidad de retiros hechos por los usuarios.");
                Console.WriteLine("2. Revisar la cantidad entregada de billetes y monedas.");
                Console.WriteLine("3. Si desea salir.");
                Console.WriteLine("Escribe:");
                int opcion = numero();
                if(opcion==1){
                    Console.WriteLine("¿Cuántos retiros se hicieron (máximo 10)?");
                    bool comprobar= false;
                    int valor = 0;
                    while(comprobar==false){
                        int valor_ = numero();
                        if(valor_<11&&valor_>0){
                            comprobar=true;
                            valor = valor_;
                        }else{
                            Console.WriteLine("Los retiros que ingresaste no esta en el intervalo permitio. \nEscribe una cantidad permitida");
                        }
                    }
                    int i=0;
                    int dinero_=0;
                    while(i<valor){
                        do{
                            Console.WriteLine($"Ingresa la cantidad del retiro #{i+1}");
                            dinero_ = numero();
                            if(dinero_>50000){
                                Console.WriteLine("La cantidad que ingresaste es mayor a $50000. Escribe con una cantidad inferior.");
                            }
                            if(dinero_<1){
                                Console.WriteLine("La cantidad que ingresaste es menor $1. Escribe con una cantidad superior.");
                            }
                            
                        }while(dinero_>50000||dinero_<1);
                        i++;
                        dinero.Add(dinero_);
                    }
                    
                    

                }
                if(opcion==2){
                    int i=1;
                    foreach (var item in dinero)
                    {
                        Console.WriteLine($"Retiro #{i}");
                        int numero_ = item;
                        int billetes_=0;
                        int monedas_= 0;
                        
                        foreach(var item2 in billetes){
                            int resultado = numero_/item2;
                            billetes_+=resultado;
                            
                            numero_=numero_-(item2*resultado);
                        }
                        foreach(var item2 in monedas){
                            int resultado = numero_/item2;
                            
                            monedas_+=resultado;
                            
                            numero_=numero_-(item2*resultado);
                        }
                        Console.WriteLine($"Billetes entregados:{billetes_}");
                        Console.WriteLine($"Monedas entregados:{monedas_}");
                        i++;
                    }
                }
                if(opcion==3){
                    check = true;
                    Console.Clear();
                }
                if((opcion!=1)&&(opcion!=2)&&(opcion!=3))
                {
                    Console.WriteLine("No ha escogido ninguna de las opciones mencianadas");
                }
                Console.WriteLine("Presiona 'enter' para continuar ...");
                Console.ReadLine();
                
            }
        }
        static int numero(){
            bool valido =false;
            int valor = 0;
            while(valido==false){
            try{
                Console.WriteLine("Escribe un numero:");
                valor = int.Parse(Console.ReadLine());
                valido =true;
            }catch(FormatException){
                Console.WriteLine("No se ha escrito correctamente. Vuelve a ingresar el dato. ");
            }}
            return valor;
            
        }
    }
}

