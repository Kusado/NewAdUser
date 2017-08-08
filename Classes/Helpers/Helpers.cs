using Helpers;
using System;
using System.Management.Automation;
using System.Net.NetworkInformation;

namespace NewAdUser {

  public static partial class Helpers {

    public static bool PingHost(string nameOrAddress, int timeout, out IPStatus status) {
      status = IPStatus.Unknown;
      bool pingable = false;
      Ping pinger = new Ping();
      try {
        PingReply reply = pinger.Send(nameOrAddress, timeout);
        status = reply.Status;
        pingable = status == IPStatus.Success;
      }
      catch (PingException) {
        // Discard PingExceptions and return false;
      }
      return pingable;
    }

    public static void AddADUser(AdUser user, PSCredential credential, Splash splash = null) {
      try {
        posh.AddUser(user, credential, splash);
      }
      catch (Exception e) {
        Console.WriteLine(e);
        throw;
      }
    }

    public static void AddMailUser(AdUser user, PSCredential credential, Splash splash = null) {
      try {
        posh.AddMailUser(user, credential, splash);
      }
      catch (Exception e) {
        Console.WriteLine(e);
        throw;
      }
    }
  }
}