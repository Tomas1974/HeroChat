using lib;

namespace a;



/******************input*********************/


public class ClientWantsToBroadcastToRoomDto : BaseDto
{
    public string message { get; set; }
    public int roomId { get; set; }
    
 
}



/******************filter*********************/


public class CategoriesAnalysis
{
    public string category { get; set; }
    public int severity { get; set; }
}

public class ContentFilterResponse
{
    public List<object> blocklistsMatch { get; set; }
    public List<CategoriesAnalysis> categoriesAnalysis { get; set; }
}


public class RequestModel 
{
    public string text { get; set; }
    public List<string> categories { get; set; }
    public string outputType { get; set; }
     
}

/******************Output*********************/



public class newMessageToStore : BaseDto
{
    public string message { get; set; }
    public string from { get; set; }
    
    public string roomId { get; set; }
}



