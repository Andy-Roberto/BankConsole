namespace BankConsole;
public class Employee : User{
    public string Department {get;set;}
    public Employee(){
        
    }
    public Employee(int ID, string Name, string Email, decimal Balance, string Department): base(ID,Name,Email,Balance){
        this.Department=Department;
        SetBalance(Balance);
    }
    public override void SetBalance(decimal amount)
    {
        base.SetBalance(amount);
        Console.WriteLine(Department);
        if(Department==null){
            Console.WriteLine("Es nulo");
        }
        if(Department!=null){
            if(Department.Equals("IT")){
                Balance += (amount*0.05m);
        }
        }
        
    }
    public override string ShowData()
    {
        return base.ShowData()+ $", Departamento: {this.Department}";
    }
}