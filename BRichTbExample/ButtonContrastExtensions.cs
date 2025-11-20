using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace ButtonBackColorExtensions
{
    internal sealed class LabelColorState
    {
        public Color OriginalBackColor { get; set; }
        public bool OverrideApplied { get; set; }
    }

    public static class LabelBackColorExtensions
    {
        private static readonly ConditionalWeakTable<Label, LabelColorState> _stateTable
            = new ConditionalWeakTable<Label, LabelColorState>();

        /// <summary>
        /// Set ForeColor and then adjust BackColor if needed:
        /// - Keep original BackColor if contrast is OK
        /// - Otherwise switch BackColor to high-contrast (black/white)
        /// </summary>
        public static void SetForeColorWithAutoBack(this Label lbl, Color foreColor)
        {
            if (lbl == null)
            {
                throw new ArgumentNullException(nameof(lbl));
            }

            lbl.ForeColor = foreColor;
            lbl.EnsureAutoBackContrast();
        }

        /// <summary>
        /// Uses the original BackColor unless the text is not readable.
        /// If contrast is bad, changes BackColor to black/white.
        /// </summary>
        public static void EnsureAutoBackContrast(this Label lbl)
        {
            if (lbl == null)
            {
                throw new ArgumentNullException(nameof(lbl));
            }

            LabelColorState state = GetOrCreateState(lbl);

            Color originalBack = state.OriginalBackColor;

            // Effective background: if Transparent, use parent's back color
            Color effectiveBack = originalBack;
            if (originalBack == Color.Transparent && lbl.Parent != null)
            {
                effectiveBack = lbl.Parent.BackColor;
            }

            Color fore = lbl.ForeColor;

            // If readable on original background → restore original (if needed) and exit
            if (IsReadable(fore, effectiveBack))
            {
                if (state.OverrideApplied)
                {
                    lbl.BackColor = state.OriginalBackColor;
                    state.OverrideApplied = false;
                }

                return;
            }

            // Not readable → choose a better background (black or white)
            Color newBack = GetHighContrastBackColor(fore);

            lbl.BackColor = newBack;
            state.OverrideApplied = true;
        }

        /// <summary>
        /// Explicitly restore the original BackColor.
        /// </summary>
        public static void RestoreOriginalBackColor(this Label lbl)
        {
            if (lbl == null)
            {
                throw new ArgumentNullException(nameof(lbl));
            }

            LabelColorState state = GetOrCreateState(lbl);

            lbl.BackColor = state.OriginalBackColor;
            state.OverrideApplied = false;
        }

        // ================== INTERNAL HELPERS ==================

        private static LabelColorState GetOrCreateState(Label lbl)
        {
            return _stateTable.GetValue(lbl, l =>
            {
                return new LabelColorState
                {
                    OriginalBackColor = l.BackColor,
                    OverrideApplied = false
                };
            });
        }

        /// <summary>
        /// "Readable enough" check using brightness difference + relaxed contrast.
        /// </summary>
        private static bool IsReadable(Color fore, Color back)
        {
            double bf = PerceivedBrightness(fore);
            double bb = PerceivedBrightness(back);

            // Brightness difference heuristic
            if (Math.Abs(bf - bb) >= 0.30)
            {
                return true;
            }

            // Fallback contrast ratio (relaxed)
            double ratio = ContrastRatio(fore, back);
            return ratio >= 2.5;
        }

        private static double PerceivedBrightness(Color c)
        {
            return (0.299 * c.R + 0.587 * c.G + 0.114 * c.B) / 255.0;
        }

        private static Color GetHighContrastBackColor(Color fore)
        {
            double contrastWithWhite = ContrastRatio(fore, Color.White);
            double contrastWithBlack = ContrastRatio(fore, Color.Black);

            return contrastWithWhite > contrastWithBlack ? Color.White : Color.Black;
        }

        private static double ContrastRatio(Color a, Color b)
        {
            double la = RelativeLuminance(a);
            double lb = RelativeLuminance(b);

            double lighter = Math.Max(la, lb);
            double darker = Math.Min(la, lb);

            return (lighter + 0.05) / (darker + 0.05);
        }

        private static double RelativeLuminance(Color c)
        {
            double R = c.R / 255.0;
            double G = c.G / 255.0;
            double B = c.B / 255.0;

            R = (R <= 0.03928) ? R / 12.92 : Math.Pow((R + 0.055) / 1.055, 2.4);
            G = (G <= 0.03928) ? G / 12.92 : Math.Pow((G + 0.055) / 1.055, 2.4);
            B = (B <= 0.03928) ? B / 12.92 : Math.Pow((B + 0.055) / 1.055, 2.4);

            return 0.2126 * R + 0.7152 * G + 0.0722 * B;
        }
    }
}
