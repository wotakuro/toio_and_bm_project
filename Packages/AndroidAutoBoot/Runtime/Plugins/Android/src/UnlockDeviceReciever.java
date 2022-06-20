package com.wotakuro.autoboot;

import android.content.Context;
import android.util.Log;

import java.lang.Exception;
import java.util.Map;
import java.util.Objects;
import android.content.BroadcastReceiver;
import android.content.Intent;
import android.content.Context;

import com.unity3d.player.UnityPlayerActivity;

public class UnlockDeviceReciever extends BroadcastReceiver  {

  
  @Override
  public void onReceive(Context context, Intent intent) {
    
    if(intent == null ){
      return;
    }
    Log.d("AutoBoot", "UnlockDeviceReciever : " + intent.getAction());
    Intent intentActivity = new Intent(context, UnityPlayerActivity.class);
    intentActivity.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
    context.startActivity(intentActivity);
    Log.d("AutoBoot", "[UnlockDeviceReciever]StartActivity ");
  }

}