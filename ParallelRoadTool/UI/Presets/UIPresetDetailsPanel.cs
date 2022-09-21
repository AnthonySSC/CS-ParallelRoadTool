﻿using System.Collections.Generic;
using AlgernonCommons.UI;
using ColossalFramework;
using ColossalFramework.UI;
using ParallelRoadTool.Managers;
using ParallelRoadTool.Models;
using ParallelRoadTool.UI.Shared;
using ParallelRoadTool.UI.Utils;
using UnityEngine;

namespace ParallelRoadTool.UI.Presets
{
    internal class UIPresetDetailsPanel : UIPanel
    {
        #region Control

        #region Public API

        public void LoadPreset(IEnumerable<XMLNetItem> networks)
        {
            var items = new FastList<object>();
            foreach (var netItem in PresetsManager.ToNetInfoItems(networks))
                items.Add(netItem);

            _netItemsList.Data = items;
        }

        #endregion

        #endregion

        private class UIPresetListRow : UIListRow
        {
            private UINetInfoTinyPanel _netInfoRow;

            public override void Display(object data, int rowIndex)
            {
                if (_netInfoRow == null)
                {
                    // Init our row
                    width = parent.width;
                    height = RowHeight;
                    isInteractive = false;

                    // Set the item
                    _netInfoRow = AddUIComponent<UINetInfoTinyPanel>();
                    _netInfoRow.relativePosition = Vector2.zero;
                    _netInfoRow.isInteractive = false;
                }

                _netInfoRow.Render((NetInfoItem)data);
                // Deselect(rowIndex);
            }
        }

        #region Unity

        #region Components

        private UIList _netItemsList;

        #endregion

        #region Lifecycle

        public UIPresetDetailsPanel()
        {
            autoLayoutDirection = LayoutDirection.Vertical;
            autoLayoutPadding = UIHelpers.RectOffsetFromPadding(0);
            autoLayout = true;
        }

        public override void Start()
        {
            base.Start();

            // We need to create this here because this panel's size is set by its container and is not know during ctor
            _netItemsList = UIList.AddUIList<UIPresetListRow>(this, 0, 0,
                                                              width,
                                                              height, UIConstants.MediumSize + 2 * UIConstants.Padding);

            // Force disable selection
            _netItemsList.EventSelectionChanged += (_, _) =>
                                                   {
                                                       _netItemsList.SelectedIndex = -1;
                                                   };
        }

        #endregion

        #endregion
    }
}