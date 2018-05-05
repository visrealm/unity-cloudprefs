package com.visualrealmsoftware.cloudprefs;

import android.app.Fragment;
import android.app.backup.BackupAgent;
import android.os.Bundle;

import com.unity3d.player.UnityPlayer;

/**
 * Created by troy on 3/05/2018.
 */

public class Plugin extends Fragment
{
    public static final String TAG = "VrsCloudPrefs.Plugin";

    public static Plugin instance;

    String gameObjectName;

    public static void init(String gameObjectName)
    {
        instance = new Plugin();
        instance.gameObjectName = gameObjectName;
        UnityPlayer.currentActivity.getFragmentManager().beginTransaction().add(instance, TAG).commit();
    }

    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setRetainInstance(true); // Retain between configuration changes (like device rotation)
    }
}
