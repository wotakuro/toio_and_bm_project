package com.wotakuro.autoboot;

import android.content.Context;
import android.util.Log;

import java.io.File;
import java.io.FileOutputStream;
import java.lang.Exception;
import java.util.Map;
import java.util.Objects;
import android.content.BroadcastReceiver;
import android.content.Intent;
import android.content.Context;

import com.unity3d.player.UnityPlayerActivity;

public class BootEventReciever extends BroadcastReceiver  {

  
  @Override
  public void onReceive(Context context, Intent intent) {
        Log.d("AutoBoot", "BootEventReciever : ${intent.action}");
    if(intent == null ||
       intent.getAction() == null ||
       !intent.getAction().equals( Intent.ACTION_BOOT_COMPLETED ) ){
      return;
    }
    if(!getEnabled(context)){
      return;
    }
    
    Intent intentActivity = new Intent(context, UnityPlayerActivity.class);
    intentActivity.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
    context.startActivity(intentActivity);
  }

  public static void setEnabled(Context context,boolean flag){
    try{
      String path = getFilePath(context);
      File file = new File(path);
      boolean current = getEnabled(context);
      if(current == flag){
        return;
      }
      if(!flag ){
        file.delete();
      }else{
        FileOutputStream fos = new FileOutputStream(file);
        fos.write(1);
        fos.flush();
        fos.close();
      }
    }catch(Exception e){
      Log.d("AutoBoot",e.toString() );
    }
    
  }

    public static boolean getEnabled(Context context){
      try{
        String path = getFilePath(context);
        File file = new File(path);
        return file.exists();
      }catch(Exception e){
        Log.d("AutoBoot",e.toString() );
        return false;
      }
  }


  private static String getFilePath(Context context){
    
    String fileName = "/autoboot_config.dat";
    String path = context.getFilesDir() + fileName;
    return path;
  }
}