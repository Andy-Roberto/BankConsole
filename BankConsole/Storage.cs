using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace BankConsole;

public static class Storage{
    static string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\user.json";
    public static void AddUser(User user){
        string json= "", json_aux= "";
        
        if(File.Exists(filePath)){
            json_aux= File.ReadAllText(filePath);
        }
        var listUsers = JsonConvert.DeserializeObject<List<object>>(json_aux);
        if(listUsers==null){
            listUsers = new List<object>();

        }
        listUsers.Add(user);
        JsonSerializerSettings settings = new JsonSerializerSettings{Formatting=Formatting.Indented};
        json = JsonConvert.SerializeObject(listUsers,settings);
        File.WriteAllText(filePath,json);
    }
    public static List<User> GetNewUsers(){
        string  json_aux= "";

        var listUsers = new List<User>();
        if(File.Exists(filePath)){
            json_aux= File.ReadAllText(filePath);
        }
        var listObjects = JsonConvert.DeserializeObject<List<object>>(json_aux);
        if(listObjects == null){
            return listUsers;
        }
        foreach(object obj in listObjects){
            User newUser;
            
            JObject user = (JObject)obj;
            if(user.ContainsKey("TaxRegime")){
                newUser = user.ToObject<Client>();
            }else{
                newUser = user.ToObject<Employee>();
            }
            listUsers.Add(newUser);
        }
        var newUsersList = listUsers.Where(user => user.GetRegisterDate().Date.Equals(DateTime.Today)).ToList();
        return newUsersList;
    }
    public static List<User> SearchUser(int ID_){
        string json= "", json_aux= "";

        var listUsers = new List<User>();
        if(File.Exists(filePath)){
            json_aux= File.ReadAllText(filePath);
        }
        var listObjects = JsonConvert.DeserializeObject<List<object>>(json_aux);
        if(listObjects == null){
            return listUsers;
        }
        foreach(object obj in listObjects){
            User newUser;
            
            JObject user = (JObject)obj;
            if(user.ContainsKey("TaxRegime")){
                newUser = user.ToObject<Client>();
            }else{
                newUser = user.ToObject<Employee>();
            }
            listUsers.Add(newUser);
        }
        var newUsersList = listUsers.Where(user => user.getId()==ID_ ).ToList();
        return newUsersList;
    }
    public static string DeleteUser(int ID){
        string json= "", json_aux= "";
        
        var listUsers = new List<User>();
        if(File.Exists(filePath)){
            json_aux= File.ReadAllText(filePath);
        }

        var listObjects = JsonConvert.DeserializeObject<List<object>>(json_aux);
        if(listObjects==null){
            return "There are no users in the file.";
        }
        List<User> lista =Storage.SearchUser(ID);
        if(lista.Count==0){
            return "No existe un usuario con ese ID";
        }
        foreach(object obj in listObjects){
            User newUser;
            
            JObject user = (JObject)obj;
            if(user.ContainsKey("TaxRegime")){
                newUser = user.ToObject<Client>();
            }else{
                newUser = user.ToObject<Employee>();
            }
            listUsers.Add(newUser);
        }
        var userToDelete = listUsers.Where(user => user.getId()==ID).Single();
        listUsers.Remove(userToDelete);
        JsonSerializerSettings settings = new JsonSerializerSettings{ Formatting= Formatting.Indented};
        json = JsonConvert.SerializeObject(listUsers,settings);
        File.WriteAllText(filePath,json);
        return "Success";
    }
}