using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.Core;
using NLog;
using NLog.Config;
using System.IO;
using System.Xml;
using NLog.Targets;
using Ghostbit.Framework.Unity.Utils;

public class SimpleExample : MonoBehaviour
{
    private Logger logger;
    private Tweaker tweaker;

    public static void Log(string level, string logger, string message)
    {
        Debug.Log(level + "|" + logger + "|" + message);
    }
    
    void Start()
    {
        // Set up logging (required by Tweaker)
        TextAsset config = Resources.Load<TextAsset>("NLog-Simple.config");
        StringReader sr = new StringReader(config.text);
        XmlReader xr = XmlReader.Create(sr);
        LogManager.Configuration = new XmlLoggingConfiguration(xr, null); 
        logger = LogManager.GetCurrentClassLogger();

        tweaker = new Tweaker();
        tweaker.Initialized += OnInitialized;
        tweaker.Init();
    }

    private void OnInitialized()
    {
        logger.Info("OnInitialized");
        tweaker.Shutdown();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
