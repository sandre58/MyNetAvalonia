// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace MyNet.Avalonia.Controls;

internal record struct HostKey(string? Id, int? Hash);

internal static class OverlayDialogManager
{
    private static readonly ConcurrentDictionary<HostKey, OverlayDialogHost> Hosts = new();

    public static void RegisterHost(OverlayDialogHost host, string? id, int? hash)
    {
        Debug.WriteLine("Count: " + Hosts.Count);
        Hosts.TryAdd(new HostKey(id, hash), host);
    }

    public static void UnregisterHost(string? id, int? hash) => Hosts.TryRemove(new HostKey(id, hash), out _);

    public static OverlayDialogHost? GetHost(string? id, int? hash)
    {
        HostKey? key = hash is null ? Hosts.Keys.FirstOrDefault(k => k.Id == id) : Hosts.Keys.FirstOrDefault(k => k.Id == id && k.Hash == hash);
        return key is null ? null : Hosts.TryGetValue(key.Value, out var host) ? host : null;
    }
}

