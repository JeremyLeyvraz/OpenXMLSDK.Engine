﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MvvX.Plugins.OpenXMLSDK.Word.ReportEngine.Models;
using MvvX.Plugins.OpenXMLSDK.Word.ReportEngine.Models.Charts;

namespace MvvX.Plugins.OpenXMLSDK.Word.ReportEngine.BatchModels
{
    /// <summary>
    /// Context 
    /// </summary>
    public class ContextModel
    {
        #region Fields

        private Dictionary<string, BaseModel> datas;
        #endregion

        #region Properties
        /// <summary>
        /// Data in context (property used for json serialization)
        /// </summary>
        public Dictionary<string, BaseModel> Data { get { return datas; } set { datas = value; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ContextModel()
        {
            datas = new Dictionary<string, BaseModel>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region Methods

        public T GetItem<T>(string key) where T : BaseModel
        {
            if (datas.ContainsKey(key))
                return (T)datas[key];
            else
                return default(T);
        }

        public void AddItem<T>(string key, T item) where T : BaseModel
        {
            if (item != null)
            {
                if (datas.ContainsKey(key))
                    datas[key] = item;
                else
                    datas.Add(key, item);
            }
        }

        public bool ExistItem<T>(string key) where T : BaseModel
        {
            return !string.IsNullOrWhiteSpace(key) && datas.ContainsKey(key) && datas[key] is T;
        }

        public string ReplaceText(string value)
        {
            var result = value;
            var pattern = @"(#[a-zA-Z\.\\_\{\}0-9]+#)";
            if (Regex.IsMatch(value, pattern))
            {
                foreach (Match match in Regex.Matches(value, pattern))
                {
                    var key = match.Value;
                    if (ExistItem<StringModel>(key))
                        result = result.Replace(key, GetItem<StringModel>(key).Value);
                    else
                        result = result.Replace(key, string.Empty);
                }
            }
            return result;
        }

        /// <summary>
        /// Replace text and visibility of element
        /// </summary>
        /// <param name="element"></param>
        public void ReplaceItem(BarModel element)
        {
            SetVisibilityFromContext(element);

            if (!string.IsNullOrEmpty(element.Title))
                element.Title = ReplaceText(element.Title);
            if (!string.IsNullOrEmpty(element.DataLabelColor))
                element.DataLabelColor = ReplaceText(element.DataLabelColor);
            if (!string.IsNullOrEmpty(element.FontName))
                element.FontName = ReplaceText(element.FontName);
            if (!string.IsNullOrEmpty(element.FontColor))
                element.FontColor = ReplaceText(element.FontColor);
            if (!string.IsNullOrEmpty(element.MajorGridlinesColor))
                element.MajorGridlinesColor = ReplaceText(element.MajorGridlinesColor);
        }

        /// <summary>
        /// Replace text and visibility of element
        /// </summary>
        /// <param name="element"></param>
        public void ReplaceItem(BaseElement element)
        {
            SetVisibilityFromContext(element);
        }

        /// <summary>
        /// Replace an image by it context value
        /// </summary>
        /// <param name="element">template image object</param>
        public void ReplaceItem(Image element)
        {
            if (!string.IsNullOrWhiteSpace(element.ContextKey))
            {
                // On va tenter de charger l'objet depuis sa base64 :
                if (ExistItem<Base64ContentModel>(element.ContextKey))
                {
                    var item = GetItem<Base64ContentModel>(element.ContextKey);
                    if (!string.IsNullOrWhiteSpace(item.Base64Content))
                        element.Content = Convert.FromBase64String(item.Base64Content);
                }
                else if (ExistItem<ByteContentModel>(element.ContextKey))
                {
                    var item = GetItem<ByteContentModel>(element.ContextKey);
                    element.Content = item.Content;
                }
                else if (ExistItem<FileLinkModel>(element.ContextKey))
                {
                    var item = GetItem<FileLinkModel>(element.ContextKey);
                    element.Path = item.Value;
                }
            }
            element.ContextKey = string.Empty;

            SetVisibilityFromContext(element);
        }

        /// <summary>
        /// Replace text, FontColor and visibility of element
        /// </summary>
        /// <param name="element"></param>
        public void ReplaceItem(Label element)
        {
            if (!string.IsNullOrEmpty(element.Text))
                element.Text = ReplaceText(element.Text);
            if (!string.IsNullOrEmpty(element.FontColor))
                element.FontColor = ReplaceText(element.FontColor);
            if (!string.IsNullOrEmpty(element.Shading))
                element.Shading = ReplaceText(element.Shading);
            if (!string.IsNullOrEmpty(element.BoldKey) && ExistItem<BooleanModel>(element.BoldKey))
            {
                var item = GetItem<BooleanModel>(element.BoldKey);
                element.Bold = item.Value;
            }
            SetVisibilityFromContext(element);
        }

        /// <summary>
        /// Replace text, FontColor and visibility of element
        /// </summary>
        /// <param name="element"></param>
        public void ReplaceItem(Paragraph element)
        {
            if (!string.IsNullOrEmpty(element.ParagraphStyleId))
                element.ParagraphStyleId = ReplaceText(element.ParagraphStyleId);
            if (!string.IsNullOrEmpty(element.FontColor))
                element.FontColor = ReplaceText(element.FontColor);
            if (!string.IsNullOrEmpty(element.Shading))
                element.Shading = ReplaceText(element.Shading);
            if (element.Borders != null && !string.IsNullOrEmpty(element.Borders.BorderColor))
                element.Borders.BorderColor = ReplaceText(element.Borders.BorderColor);
            if (!string.IsNullOrEmpty(element.BoldKey) && ExistItem<BooleanModel>(element.BoldKey))
            {
                var item = GetItem<BooleanModel>(element.BoldKey);
                element.Bold = item.Value;
            }
            SetVisibilityFromContext(element);
        }

        /// <summary>
        /// Replace text, FontColor and visibility of element
        /// </summary>
        /// <param name="element"></param>
        public void ReplaceItem(Cell element)
        {
            if (!string.IsNullOrEmpty(element.FontColor))
                element.FontColor = ReplaceText(element.FontColor);
            if (!string.IsNullOrEmpty(element.Shading))
                element.Shading = ReplaceText(element.Shading);
            if (element.Borders != null && !string.IsNullOrEmpty(element.Borders.BorderColor))
                element.Borders.BorderColor = ReplaceText(element.Borders.BorderColor);
            if (!string.IsNullOrEmpty(element.FusionKey) && ExistItem<BooleanModel>(element.FusionKey))
            {
                var item = GetItem<BooleanModel>(element.FusionKey);
                element.Fusion = item.Value;
            }
            if (!string.IsNullOrEmpty(element.FusionChildKey) && ExistItem<BooleanModel>(element.FusionChildKey))
            {
                var item = GetItem<BooleanModel>(element.FusionChildKey);
                element.FusionChild = item.Value;
            }
            SetVisibilityFromContext(element);
        }

        /// <summary>
        /// Replace text, FontColor and visibility of element
        /// </summary>
        /// <param name="element"></param>
        public void ReplaceItem(Table element)
        {
            if (!string.IsNullOrEmpty(element.FontColor))
                element.FontColor = ReplaceText(element.FontColor);
            if (!string.IsNullOrEmpty(element.Shading))
                element.Shading = ReplaceText(element.Shading);
            if (element.Borders != null && !string.IsNullOrEmpty(element.Borders.BorderColor))
                element.Borders.BorderColor = ReplaceText(element.Borders.BorderColor);
            SetVisibilityFromContext(element);
        }

        private void SetVisibilityFromContext(BaseElement element)
        {
            if (!string.IsNullOrWhiteSpace(element.ShowKey) && ExistItem<BooleanModel>(element.ShowKey))
            {
                var item = GetItem<BooleanModel>(element.ShowKey);
                element.Show = item.Value;
            }
            element.ShowKey = string.Empty;
        }
        #endregion
    }
}
