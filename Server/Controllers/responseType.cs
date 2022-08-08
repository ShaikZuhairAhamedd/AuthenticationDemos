namespace Server.Controllers
{

    // what are the feild are to be required for  the 
    // responseType for the  Oauth/token endPoint
    /*
        {There are Some feilds like RefreshToken,Expries etc....,are to be discussed}
     */
    // Please Read The Documentaation    

    internal class responseType
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string raw_claim { get; set; }
    }
}