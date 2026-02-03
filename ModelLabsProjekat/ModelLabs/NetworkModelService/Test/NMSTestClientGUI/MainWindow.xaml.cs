using FTN.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TelventDMS.Services.NetworkModelService.TestClient.Tests;

namespace NMSTestClientGUI
{
    public partial class MainWindow : Window
    {
        private TestGda testGda;

        public MainWindow()
        {
            InitializeComponent();
            testGda = new TestGda();

            ApplyPlaceholder(TbGetValuesGid, "npr. 0x123456789");
            ApplyPlaceholder(TbExtentModelCode, "npr. ASSET ili SEAL");
            ApplyPlaceholder(TbSourceGid, "Enter Source GID");
            ApplyPlaceholder(TbAssociationProperty, "Enter Property");
            ApplyPlaceholder(TbAssociationType, "Enter Association Type");
            ApplyPlaceholder(TbSealKind, "npr. steel / lock");
            ApplyPlaceholder(TbAssetType, "npr. meter / box");
        }

        private void ApplyPlaceholder(TextBox box, string text)
        {
            box.Text = text;
            box.Foreground = Brushes.Gray;

            box.GotFocus += (s, e) =>
            {
                if (box.Text == text)
                {
                    box.Text = "";
                    box.Foreground = Brushes.White;
                }
            };

            box.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {
                    box.Text = text;
                    box.Foreground = Brushes.Gray;
                }
            };
        }

        // --------------------
        // Koleginicine tri metode (IDENTIČNE)
        // --------------------
        private void BtnGetValues_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long gid = ParseGlobalId(TbGetValuesGid.Text);
                var rd = testGda.GetValues(gid);

                RtbGetValues.Document.Blocks.Clear();
                RtbGetValues.Document.Blocks.Add(new Paragraph(new Run(rd.ToString())));
            }
            catch (Exception ex)
            {
                ShowError(RtbGetValues, ex.Message);
            }
        }

        private void BtnGetExtentValues_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ModelCode modelCode;
                if (!ModelCodeHelper.GetModelCodeFromString(TbExtentModelCode.Text, out modelCode))
                    throw new Exception("Invalid ModelCode");

                var ids = testGda.GetExtentValues(modelCode);
                string text = GetExtentValuesToString(ids, testGda);

                RtbExtentValues.Document.Blocks.Clear();
                RtbExtentValues.Document.Blocks.Add(new Paragraph(new Run(text)));
            }
            catch (Exception ex)
            {
                ShowError(RtbExtentValues, ex.Message);
            }
        }

        private void BtnGetRelatedValues_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long sourceGid = ParseGlobalId(TbSourceGid.Text);

                ModelCode propId;
                ModelCode assocType;

                ModelCodeHelper.GetModelCodeFromString(TbAssociationProperty.Text, out propId);
                ModelCodeHelper.GetModelCodeFromString(TbAssociationType.Text, out assocType);

                Association assoc = new Association
                {
                    PropertyId = propId,
                    Type = assocType
                };

                var ids = testGda.GetRelatedValues(sourceGid, assoc);
                string text = GetExtentValuesToString(ids, testGda);

                RtbRelatedValues.Document.Blocks.Clear();
                RtbRelatedValues.Document.Blocks.Add(new Paragraph(new Run(text)));
            }
            catch (Exception ex)
            {
                ShowError(RtbRelatedValues, ex.Message);
            }
        }

        // --------------------
        // Dodatne metode za tvoj domen
        // --------------------
        private void BtnGetSealByKind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sealIds = testGda.GetExtentValues(ModelCode.SEAL);
                string kindFilter = TbSealKind.Text.Trim().ToLower();

                StringBuilder sb = new StringBuilder();
                foreach (var gid in sealIds)
                {
                    var rd = testGda.GetValues(gid);
                    var kindProp = rd.GetProperty(ModelCode.SEAL_KIND);
                    if (kindProp != null && kindProp.PropertyValue.ToString().ToLower().Contains(kindFilter))
                        sb.AppendLine(rd.ToString());
                }

                RtbSealKind.Document.Blocks.Clear();
                RtbSealKind.Document.Blocks.Add(new Paragraph(new Run(sb.ToString())));
            }
            catch (Exception ex)
            {
                ShowError(RtbSealKind, ex.Message);
            }
        }

        private void BtnMinProgramId_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ids = testGda.GetExtentValues(ModelCode.ASSETFUNCTION);
                string shortest = null;
                long chosen = -1;

                foreach (var gid in ids)
                {
                    var rd = testGda.GetValues(gid);
                    var prop = rd.GetProperty(ModelCode.ASSETFUNCTION_PROGRAMID);
                    if (prop == null) continue;

                    string val = prop.AsString();
                    if (shortest == null || val.Length < shortest.Length)
                    {
                        shortest = val;
                        chosen = gid;
                    }
                }

                if (chosen == -1)
                    throw new Exception("No AssetFunction found!");

                var result = testGda.GetValues(chosen);
                RtbMinProgram.Document.Blocks.Clear();
                RtbMinProgram.Document.Blocks.Add(new Paragraph(new Run(result.ToString())));
            }
            catch (Exception ex)
            {
                ShowError(RtbMinProgram, ex.Message);
            }
        }

        private void BtnFilterAssets_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ids = testGda.GetExtentValues(ModelCode.ASSET);
                string typeFilter = TbAssetType.Text.Trim().ToLower();

                StringBuilder sb = new StringBuilder();
                foreach (var gid in ids)
                {
                    var rd = testGda.GetValues(gid);
                    var prop = rd.GetProperty(ModelCode.ASSET_TYPE);
                    if (prop != null && prop.AsString().ToLower().Contains(typeFilter))
                        sb.AppendLine(rd.ToString());
                }

                RtbAssetType.Document.Blocks.Clear();
                RtbAssetType.Document.Blocks.Add(new Paragraph(new Run(sb.ToString())));
            }
            catch (Exception ex)
            {
                ShowError(RtbAssetType, ex.Message);
            }
        }

        // --------------------
        // Helpers
        // --------------------
        private long ParseGlobalId(string text)
        {
            if (text.StartsWith("0x"))
                return Convert.ToInt64(text.Substring(2), 16);
            return Convert.ToInt64(text);
        }

        private void ShowError(RichTextBox box, string msg)
        {
            box.Document.Blocks.Clear();
            box.Document.Blocks.Add(new Paragraph(new Run("ERROR: " + msg)));
        }

        private static string GetExtentValuesToString(List<long> ids, TestGda gda)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var id in ids)
            {
                sb.AppendLine(gda.GetValues(id).ToString());
            }
            return sb.ToString();
        }
    }
}