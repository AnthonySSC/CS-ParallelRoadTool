﻿// <copyright file="Mod.cs" company="ST-Apps (S. Tenuta)">
// Copyright (c) ST-Apps (S. Tenuta). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace ParallelRoadTool;

using AlgernonCommons;
using AlgernonCommons.Notifications;
using AlgernonCommons.Patching;
using AlgernonCommons.Translation;
using ColossalFramework.UI;
using HarmonyLib;
using ICities;
using Settings;
using UI.Settings;

/// <summary>
///     The base mod class for instantiation by the game.
/// </summary>
public sealed class Mod : PatcherMod<UIOptionsPanel, PatcherBase>, IUserMod
{
    /// <summary>
    ///     Minimum minor version that is compatible with the mod.
    /// </summary>
    private const string CompatibleVersion = "1.18";

    /// <summary>
    ///     Simplified name, used for file-system operations.
    /// </summary>
    public static string SimplifiedName => "Parallel Road Tool";

#if DEBUG
    /// <summary>
    ///     Gets the mod's base display name (name only).
    ///     For DEBUG builds we also include the current branch name.
    /// </summary>
    public override string BaseName => "[BETA] Parallel Road Tool";
#else
        /// <summary>
        /// Gets the mod's base display name (name only).
        /// </summary>
        public override string BaseName => "Parallel Road Tool";
#endif

    /// <summary>
    ///     Gets the mod's what's new message array.
    /// </summary>
    public override WhatsNewMessage[] WhatsNewMessages => WhatsNewMessageListing.Messages;

    /// <summary>
    ///     Gets the mod's unique Harmony identifier.
    /// </summary>
    public override string HarmonyID => "it.stapps.cities.parallelroadtool";

    /// <summary>
    ///     Gets the mod's description for display in the content manager.
    /// </summary>
    public string Description => Translations.Translate("MOD_DESCRIPTION");

    /// <summary>
    ///     Called by the game when the mod is enabled.
    /// </summary>
    public override void OnEnabled()
    {
#if DEBUG
        Harmony.DEBUG = true;
#endif

        // Disable mod if version isn't compatible.
        if (!BuildConfig.applicationVersion.StartsWith(CompatibleVersion))
        {
            Logging.Error("invalid game version detected!");

            // Display error message.
            // First, check to see if UIView is ready.
            if (UIView.GetAView() != null)
            {
                // It's ready - attach the hook now.
                DisplayVersionError();
            }
            else
            {
                // Otherwise, queue the hook for when the intro's finished loading.
                LoadingManager.instance.m_introLoaded += DisplayVersionError;
            }

            // Don't do anything else - no options panel hook, no Harmony patching.
            return;
        }

        // All good - continue as normal.
        base.OnEnabled();
    }

    /// <summary>
    ///     Saves settings file.
    /// </summary>
    public override void SaveSettings()
    {
        ModSettings.Save();
    }

    /// <summary>
    ///     Loads settings file.
    /// </summary>
    public override void LoadSettings()
    {
        ModSettings.Load();
    }

    /// <summary>
    ///     Displays a version incompatibility error.
    /// </summary>
    private static void DisplayVersionError()
    {
        var versionErrorNotification = NotificationBase.ShowNotification<ListNotification>();
        versionErrorNotification.AddParas(
            Translations.Translate("WRONG_VERSION"),
            Translations.Translate("SHUT_DOWN"),
            BuildConfig.applicationVersion);
    }
}
