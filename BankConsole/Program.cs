using System.Text.RegularExpressions;
using BankConsole;
if(args.Length ==0){
    EmailService.SendMail();
}else{
    ShowMenu();
}
void ShowMenu(){
    // Console.Clear();
    Console.WriteLine("Selecciona una opción:");
    Console.WriteLine("1 - Crear un Usuario nuevo.");


    Console.WriteLine("2 - Eliminar un Usuario existente.");
    Console.WriteLine("3 - Salir.");
    int option =0;
    do{
        string input = Console.ReadLine();
        if(!int.TryParse(input,out option))
        {
            Console.WriteLine("Debes ingresar un número (1, 2 o 3).");
        }else if(option>3){
            Console.WriteLine("Debes ingresar un número válido (1, 2 o 3)");
        }
    }while(option==0 || option >3 || option<0);
    switch(option){
        case 1:
            CreateUser();break;
        case 2:
            DeleteUser();break;
        case 3:
            Environment.Exit(0);break;
    }

}
void CreateUser(){
    Console.Clear();
    Console.WriteLine("Ingresa la información del usuario:");
    var test = true;
    int ID = 0;
    do{
        Console.Write("ID: ");
        ID = (int)numero();
        
        List<User> lista =Storage.SearchUser(ID);
        if(ID<1||lista.Count>0){
            Console.WriteLine("El ID ya esta en uso o no escribio un numero entero positivo. Vuelva a escribir.");
            test=false;
        }else{
            test=true;
        }
    }while(test==false);
        Console.Write("Nombre:");
        string name = Console.ReadLine();
        string email = "";
     do{
        Console.Write("Email:"); 
        email = Console.ReadLine();
        if(!Regex.IsMatch(email,"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")){
                Console.WriteLine("El correo escrito tiene un error. Vuelve a escribir");
                test=false; 
            }else{
                test=true;
            }
    }while(test==false);
    decimal balance = 0;
    char userType = '0';
    do{
        Console.Write("Saldo:");
        balance = (decimal)numero();
        if(balance <0m){
            Console.WriteLine("El saldo escrito es incorrecto. Vuelva a escribirlo.");
        }
    }while(balance<0m);
    test = false;
    do{
        Console.Write("Escribe 'c' si el usuario es Cliente, 'e' si es Empleado:");
        userType = char.Parse(Console.ReadLine());
        if(userType.Equals('c')||userType.Equals('e')){
            test= true;
        }else{
            Console.WriteLine("No ha escrito ninguno de los tipos de usuario.Vuelva a escribir.");
        }
    }while(test==false);
        
    
    User newUser;
    if(userType.Equals('c')){
        Console.Write("Regimen Fiscal: ");
        char taxRegime = char.Parse(Console.ReadLine());
        newUser = new Client(ID,name, email,balance, taxRegime);

    }else{
        Console.Write("Departamento:");
        string department = Console.ReadLine();
        newUser = new Employee(ID,name,email,balance,department);

    }
    Storage.AddUser(newUser);
    Console.WriteLine("Usuario creado.");
    Thread.Sleep(2000);
    ShowMenu();

}
void DeleteUser(){
    Console.Clear();
    Console.Write("Ingresa el ID del usuario a eliminar: ");
    bool test = false;
    int ID=0;
    do{
        ID = (int)numero();
        if(ID<0){
            Console.WriteLine("Has escrito un numero negativo.Escribe de nuevo.");
        }else{
            test=true;
        }
    }while(test==false);
    string result = Storage.DeleteUser(ID);
    if(result.Equals("Success")){
        Console.Write("Usuario eliminado.");
        Thread.Sleep(2000);
        ShowMenu();
    }
    else{
        Console.Write(result);
        Thread.Sleep(2000);
        Console.Clear();
        ShowMenu();
    }

}
static float numero(){
            bool valido =false;
            float valor = 0;
            while(valido==false){
            try{
                valor = float.Parse(Console.ReadLine());
                valido =true;
            }catch(FormatException){
                Console.WriteLine("No se ha escrito un número. Vuelve a ingresar el dato. ");
            }}
            return valor;

        }
