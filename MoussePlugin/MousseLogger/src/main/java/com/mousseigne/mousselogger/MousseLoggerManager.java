package com.mousseigne.mousselogger;
import android.util.Log;

public class MousseLoggerManager
{
    private static final MousseLoggerManager instance = new MousseLoggerManager();

    public static MousseLoggerManager getInstance()
    {
        return instance;
    }

    public long getTime()
    {
        return System.currentTimeMillis();
    }

    public void sendLog(String msg)
    {
        Log.i("MousseLOG", msg);
    }
}
