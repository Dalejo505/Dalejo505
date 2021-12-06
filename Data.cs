
using System.Xml.Serialization;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class Data : MonoBehaviour
{
  public struct Datos{
      public string name;
      public string image;
      public string fbx;

      
  }
  
    [SerializeField] Text uiName;
    [SerializeField] RawImage uiImage;

    [SerializeField] GameObject fbx;

    
    
    public string rutafbx;
    
    

    string JsonUrl="https://drive.google.com/uc?export=download&id=1JnNLA_MH1SMiyfagUeEmu1SfmyC8ztW6";
     

    void Start()
   {
       //StartCoroutine(GetJsonName(JsonUrl));
       StartCoroutine(BajarModelo(JsonUrl));
       
      
       
       
       

       

       
       
   }
 
    
    IEnumerator GetJsonName(string url){
        UnityWebRequest request= UnityWebRequest.Get(url);

        yield return request.Send();

        if(request.isError){

        }
        else{
            Datos datos = JsonUtility.FromJson<Datos>(request.downloadHandler.text);
            uiName.text= datos.name;
            StartCoroutine(GetJsonImage(datos.image));
        }
        request.Dispose();
        
    }
    IEnumerator GetJsonImage(string url){
        UnityWebRequest request= UnityWebRequestTexture.GetTexture(url);

        yield return request.Send();

        if(request.isError){

        }
        else{
            uiImage.texture=((DownloadHandlerTexture)request.downloadHandler).texture;
            
            

        }
        request.Dispose();
        
    }
    IEnumerator downloadAsset(string url)
{
    
    
    UnityWebRequest www = UnityWebRequest.Get(url);
    DownloadHandler handle = www.downloadHandler;

    //Send Request and wait
    yield return www.Send();

    if (www.isError)
    {

        UnityEngine.Debug.Log("Error : " + www.error);
    }
    else
    {
        UnityEngine.Debug.Log("Success");

        //handle.data

        //Construct path to save it
        string dataFileName = "model";
        string tempPath = Path.Combine(Application.persistentDataPath, "AssetData");
        tempPath = Path.Combine(tempPath, dataFileName + ".fbx");
        

        //Save
        save(handle.data, tempPath);
       
       AssetBundleCreateRequest bundle =AssetBundle.LoadFromFileAsync(tempPath);
       AssetBundle Loadmodel=bundle.assetBundle;

       GameObject obj = Loadmodel.LoadAsset<GameObject>("model") as GameObject;

        Instantiate(obj);
       
        
        
    }
}

void save(byte[] data, string path)
{
    //Create the Directory if it does not exist
    if (!Directory.Exists(Path.GetDirectoryName(path)))
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
    }

    try
    {
        File.WriteAllBytes(path, data);
        Debug.Log("Saved Data to: " + path.Replace("/", ""));
        
    }
    catch (Exception e)
    {
        Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", ""));
        Debug.LogWarning("Error: " + e.Message);
    }
}   


IEnumerable LoadObject(string path)
{

    //AssetBundle bundle= AssetBundle.LoadFromFileAsync(path).assetBundle;
    AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(path);
    
    yield return bundle;
    

    AssetBundle myLoadedAssetBundle = bundle.assetBundle;
    //AssetBundle myLoadedAssetBundle = bundle;
    if (myLoadedAssetBundle == null)
    {
        Debug.Log("Fallo al encontrar modelo!");
        yield break;
    }
    else{

    AssetBundleRequest request = myLoadedAssetBundle.LoadAssetAsync<GameObject>("model");
    yield return request;
    
    GameObject obj = request.asset as GameObject;
    obj.transform.position = new Vector3(0.08f, -2.345f, 297.54f);
    obj.transform.Rotate(350.41f, 400f, 20f);
    obj.transform.localScale = new Vector3(1.0518f, 0.998f, 1.1793f);

    Instantiate(obj);
    Debug.Log("Fbx creado");

    myLoadedAssetBundle.Unload(false);
    }
}



IEnumerator BajarModelo(string url){
    
      UnityWebRequest request= UnityWebRequest.Get(url);

        yield return request.Send();

        if(request.isError){

        }
        else{
            
            Datos datos = JsonUtility.FromJson<Datos>(request.downloadHandler.text);
            rutafbx= datos.fbx;
            Debug.Log(rutafbx);
            StartCoroutine(downloadAsset(rutafbx));
            
           
            
            
            
        }
        request.Dispose();
        
}
IEnumerator loadmodel2(string url){
    
    UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);

    yield return request.SendWebRequest();

    if(request.result!=UnityWebRequest.Result.Success){
        Debug.Log("Error");
    }
    else{
        AssetBundle bundle= DownloadHandlerAssetBundle.GetContent(request);
        Debug.Log("Modelo listo");
    }


}



    


    
    

  

    
    
   
}
