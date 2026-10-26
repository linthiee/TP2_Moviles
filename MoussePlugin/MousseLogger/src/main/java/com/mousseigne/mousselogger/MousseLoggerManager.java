package com.mousseigne.mousselogger;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.util.Log;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

public class MousseLoggerManager
{
    private static final MousseLoggerManager instance = new MousseLoggerManager();

    public static MousseLoggerManager getInstance()
    {
        return instance;
    }

    private Activity getUnityActivity()
    {
        try
        {
            Class<?> unityPlayerClass = Class.forName("com.unity3d.player.UnityPlayer");
            java.lang.reflect.Field currentActivityField = unityPlayerClass.getField("currentActivity");

            return (Activity)
                    currentActivityField.get(null);

        } catch (Exception e)
        {
            return null;
        }
    }

    private File getLogFile()
    {
        Activity activity = getUnityActivity();
        if (activity != null)
            return new File(activity.getExternalFilesDir(null), "mousse_logs.txt");

        return null;
    }

    public void sendLog(String msg)
    {
        Log.i("MousseLOG", msg);
        try
        {
            File logFile = getLogFile();
            if (logFile != null)
            {
                FileWriter writer = new FileWriter(logFile, true);
                writer.append(msg).append("\n");
                writer.flush();
                writer.close();
            }
        } catch (IOException e)
        {
            Log.e("MousseLOG", "Error guardando: " + e.getMessage());
        }
    }

    public String getAllLogs()
    {
        StringBuilder logs = new StringBuilder();
        try
        {
            File logFile = getLogFile();
            if (logFile != null && logFile.exists())
            {
                BufferedReader br = new BufferedReader(new FileReader(logFile));
                String line;

                while ((line = br.readLine()) != null)
                    logs.append(line).append("\n");

                br.close();
            } else
            {
                logs.append("Todavia no hay logs registrados\n");
            }
        } catch (IOException e)
        {
            return "Error al leer los logs nativos";
        }
        return logs.toString();
    }

    public void requestClearLogs()
    {
        final Activity activity = getUnityActivity();
        if (activity == null)
            return;

        activity.runOnUiThread(new Runnable()
        {
            @Override
            public void run()
            {
                new AlertDialog.Builder(activity)
                        .setTitle("Alerta del Sistema")
                        .setMessage("Estas seguro de que queres borrar todos los logs registrados por el juego?")
                        .setPositiveButton("Sí, limpiar", new DialogInterface.OnClickListener()
                        {
                            public void onClick(DialogInterface dialog, int which)
                            {
                                File logFile = getLogFile();
                                if (logFile != null && logFile.exists())
                                {
                                    logFile.delete();
                                }
                                notifyUnity();
                            }
                        })
                        .setNegativeButton("Cancelar", null)
                        .setIcon(android.R.drawable.ic_dialog_alert)
                        .show();
            }
        });
    }

    private void notifyUnity()
    {
        try
        {
            Class<?> unityClass = Class.forName("com.unity3d.player.UnityPlayer");
            java.lang.reflect.Method method = unityClass.getMethod("UnitySendMessage", String.class, String.class, String.class);
            method.invoke(null, "MousseLoggerObj", "OnLogsCleared", "");
        } catch (Exception e)
        {
            Log.e("MousseLOG", "Error en callback: " + e.getMessage());
        }
    }
}