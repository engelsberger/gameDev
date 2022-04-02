/* HANDLES ALL GRAPHICS SETTINGS
 *
 * This should be the only script that can modify the actual graphics settings in Unity.
 * It's designed to make changing the settings from another script very simple, since you just have to call the
 * corresponding function and provide a new value. Values like resolution or antiAliasing are predefined in public enums.
 * The functions you can call from elsewhere that can change graphics settings are:
 *
 * SetGraphicsQuality(GraphicQuality quality)
 * SetFullscreen(bool fullScreen)
 * ChangeResolution(ResolutionType resolution)
 * ChangeRefreshRate(RefreshRateType refreshRate)
 * SetVSync(bool vSync)
 * SetAntiAliasing(AntiAliasingType antiAliasing)
 */


using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; //Don't forget to get PostProcessing from Unity package manager


#region Graphics settings enums
public enum ResolutionType
{
    notAssigned,

    //16:10
    r1280x800,
    r1440x900,
    r1680x1050,
    r1920x1200,

    //16:9
    r1280x720,
    r1366x768,
    r1600x900,
    r1920x1080,

    //4:3
    r800x600,
    r1024x768
}

public enum RefreshRateType
{
    notAssigned,
    r60Hz,
    r120Hz,
    r144Hz
}

public enum GraphicQuality //Texture and Shadows
{
    notAssigned,
    veryLow,
    low,
    medium,
    high,
    veryHigh,
    ultra
}

public enum AntiAliasingType
{
    notAssigned,
    none,
    FXAA,
    MSAA2X,
    MSAA4X,
    MSAA8X
}
#endregion

#region Graphics settings classes
public class Resolution
{
    public ResolutionType type;
    public int screenWidth;
    public int screenHeight;

    public Resolution(ResolutionType type, int screenWidth, int screenHeight)
    {
        this.type = type;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
    }
}

public class RefreshRate
{
    public RefreshRateType type;
    public int rate;

    public RefreshRate(RefreshRateType type, int rate)
    {
        this.type = type;
        this.rate = rate;
    }
}

public class AntiAliasing
{
    public AntiAliasingType type;
    public int msaa;
    public PostProcessLayer.Antialiasing fxaa;

    public AntiAliasing(AntiAliasingType type, int msaa, PostProcessLayer.Antialiasing fxaa)
    {
        this.type = type;
        this.msaa = msaa;
        this.fxaa = fxaa;
    }
}

public class Settings
{
    public GraphicQuality quality;
    public Resolution resolution;
    public RefreshRate refreshRate;
    public int fullScreen;  //1 .. true, 0 .. false, -1 .. notAssigned
    public int vSync;       //1 .. true, 0 .. false, -1 .. notAssigned
    public AntiAliasing antiAliasing;

    //Set Default values
    public Settings()
    {
        quality = GraphicQuality.notAssigned;
        resolution = new Resolution(ResolutionType.notAssigned, -1, -1);
        refreshRate = new RefreshRate(RefreshRateType.notAssigned, -1);
        fullScreen = -1;
        vSync = -1;
        antiAliasing = new AntiAliasing(AntiAliasingType.notAssigned, -1, PostProcessLayer.Antialiasing.None);
    }

    //Update Settings: Create a Settings x = new Settings(), set the values that need to be changed and call this function which updates only the values that were modified.
    public void UpdateSettings(Settings newSettings)
    {
        if (newSettings.quality != GraphicQuality.notAssigned) quality = newSettings.quality;
        if (newSettings.resolution.type != ResolutionType.notAssigned) resolution = newSettings.resolution;
        if (newSettings.refreshRate.type != RefreshRateType.notAssigned) refreshRate = newSettings.refreshRate;
        if (newSettings.fullScreen != -1) fullScreen = newSettings.fullScreen;
        if (newSettings.vSync != -1) vSync = newSettings.vSync;
        if (newSettings.antiAliasing.type != AntiAliasingType.notAssigned) antiAliasing = newSettings.antiAliasing;
    }
}
#endregion



public class GraphicsSettings : MonoBehaviour
{
    //Delegate
    public delegate void SettingsChanged();
    public SettingsChanged settingsChangedCallback;

    //Cache
    private Settings settings = new Settings();
    private PostProcessLayer camPostProcessLayer;

    //Predefined Setting Types
    private List<Resolution> resolutions = new List<Resolution>() { 
        //16:10
        new Resolution(ResolutionType.r1280x800, 1280, 800),
        new Resolution(ResolutionType.r1440x900, 1440, 900),
        new Resolution(ResolutionType.r1680x1050, 1680, 1050),
        new Resolution(ResolutionType.r1920x1200, 1920, 1200),

        //16:9
        new Resolution(ResolutionType.r1280x720, 1280, 720),
        new Resolution(ResolutionType.r1366x768, 1366, 768),
        new Resolution(ResolutionType.r1600x900, 1600, 900),
        new Resolution(ResolutionType.r1920x1080, 1920, 1080),

        //4:3
        new Resolution(ResolutionType.r1920x1080, 800, 600),
        new Resolution(ResolutionType.r1920x1080, 1024, 768)
    };
    private List<RefreshRate> refreshRates = new List<RefreshRate>()
    {
        new RefreshRate(RefreshRateType.r60Hz, 60),
        new RefreshRate(RefreshRateType.r120Hz, 120),
        new RefreshRate(RefreshRateType.r144Hz, 144)
    };
    private List<AntiAliasing> antiAliasings = new List<AntiAliasing>()
    {
        new AntiAliasing(AntiAliasingType.none, 0, PostProcessLayer.Antialiasing.None),
        new AntiAliasing(AntiAliasingType.FXAA, 0, PostProcessLayer.Antialiasing.FastApproximateAntialiasing),
        new AntiAliasing(AntiAliasingType.MSAA2X, 2, PostProcessLayer.Antialiasing.None),
        new AntiAliasing(AntiAliasingType.MSAA4X, 4, PostProcessLayer.Antialiasing.None),
        new AntiAliasing(AntiAliasingType.MSAA8X, 8, PostProcessLayer.Antialiasing.None)
    };



    private void Start()
    {
        /* START - SET POST PROCESS LAYER AND TRY TO LOAD SETTINGS
         * 
         * Don't forget to add a PostProcessLayer to the main camera.
         */

        camPostProcessLayer = Camera.main.GetComponent<PostProcessLayer>();

        if (!LoadSettings())
        {
            SetAllDefaults();
        }
    }

    #region Save/Load
    public void SaveSettings()
    {
        // SAVE SETTINGS - TO BE DONE
    }

    public bool LoadSettings()
    {
        // LOAD SETTINGS - TO BE DONE

        return false;
    }
    #endregion



    public Settings GetSettings()
    {
        /* GET SETTINGS - RETURN CURRENT SETTINGS
         * 
         * In case another script needs the current settings, for example the UI manager that has to display them.
         */

        return settings;
    }

    private void ChangeSettings(Settings newSettings)
    {
        // CHANGE SETTINGS - CHANGES ACTUAL SETTINGS AND UPDATES SETTINGS VARIABLE

        // Update settings cache
        settings.UpdateSettings(newSettings);

        // Set Unity graphics quality
        if (newSettings.quality != GraphicQuality.notAssigned)
        {
            QualitySettings.SetQualityLevel((int)newSettings.quality - 1, true);
        }

        // Set screen resolution, refresh rate and fullscreen settings (set with one command in Unity)
        if (newSettings.resolution.type != ResolutionType.notAssigned || newSettings.refreshRate.type != RefreshRateType.notAssigned || newSettings.fullScreen != -1)
        {
            int screenWidth = settings.resolution.screenWidth;
            int screenHeight = settings.resolution.screenHeight;
            if (newSettings.resolution.type != ResolutionType.notAssigned) {
                screenWidth = resolutions.Where(r => r.type.Equals(newSettings.resolution.type)).Select(r => r.screenWidth).First();
                screenHeight = resolutions.Where(r => r.type.Equals(newSettings.resolution.type)).Select(r => r.screenHeight).First();
            }

            int refreshRate = settings.refreshRate.rate;
            if (newSettings.refreshRate.type != RefreshRateType.notAssigned) refreshRate = refreshRates.Where(r => r.type.Equals(newSettings.refreshRate.type)).Select(r => r.rate).First();

            bool fullScreen = Convert.ToBoolean(settings.fullScreen);
            if (newSettings.fullScreen != -1) fullScreen = Convert.ToBoolean(newSettings.fullScreen);

            Screen.SetResolution(screenWidth, screenHeight, fullScreen, refreshRate);
        }

        // Activate or deactivate vSync
        if (newSettings.vSync != -1) QualitySettings.vSyncCount = newSettings.vSync;

        // Set Unity anti aliasing settings
        if (newSettings.antiAliasing.type != AntiAliasingType.notAssigned)
        {
            QualitySettings.antiAliasing = antiAliasings.Where(a => a.type.Equals(newSettings.antiAliasing.type)).Select(a => a.msaa).First();
            camPostProcessLayer.antialiasingMode = antiAliasings.Where(a => a.type.Equals(newSettings.antiAliasing.type)).Select(a => a.fxaa).First();
        }

        // Invoke settings changed callback event, so that all scripts dependant on this will get notified
        if (settingsChangedCallback != null) { settingsChangedCallback.Invoke(); }
    }



    #region Change graphic settings functions
    public void SetGraphicsQuality(GraphicQuality quality)
    {
        Settings newSettings = new Settings();
        newSettings.quality = quality;
        ChangeSettings(newSettings);
    }

    public void SetFullscreen(bool fullScreen)
    {
        Settings newSettings = new Settings();
        newSettings.fullScreen = Convert.ToInt32(fullScreen);
        ChangeSettings(newSettings);
    }

    public void ChangeResolution(ResolutionType resolution)
    {
        Settings newSettings = new Settings();
        newSettings.resolution = resolutions.Where(r => r.type.Equals(resolution)).First();
        ChangeSettings(newSettings);
    }

    public void ChangeRefreshRate(RefreshRateType refreshRate)
    {
        Settings newSettings = new Settings();
        newSettings.refreshRate = refreshRates.Where(r => r.type.Equals(refreshRate)).First();
        ChangeSettings(newSettings);
    }

    public void SetVSync(bool vSync)
    {
        Settings newSettings = new Settings();
        newSettings.vSync = Convert.ToInt32(vSync);
        ChangeSettings(newSettings);
    }

    public void SetAntiAliasing(AntiAliasingType antiAliasing)
    {
        Settings newSettings = new Settings();
        newSettings.antiAliasing = antiAliasings.Where(a => a.type.Equals(antiAliasing)).First();
        ChangeSettings(newSettings);
    }
    #endregion



    public void SetAllDefaults()
    {
        /* SETS ALL SETTINGS TO DEFAULT
         * 
         * What the settings are set to by default does not matter, it's just important that they are set to something.
         * This option triggers if no settings could be loaded.
         */

        Settings newSettings = new Settings();

        newSettings.quality = GraphicQuality.medium;
        newSettings.resolution = resolutions.Where(r => r.type.Equals(ResolutionType.r1920x1080)).First();
        newSettings.refreshRate = refreshRates.Where(r => r.type.Equals(RefreshRateType.r60Hz)).First();
        newSettings.fullScreen = 1;
        newSettings.vSync = 1;
        newSettings.antiAliasing = antiAliasings.Where(a => a.type.Equals(AntiAliasingType.FXAA)).First();

        ChangeSettings(newSettings);
    }
}
