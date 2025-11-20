using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace ButtonBackColorExtensions
{
    internal sealed class ButtonColorState
    {
        public Color OriginalBackColor { get; set; }
        public bool OriginalUseVisualStyleBackColor { get; set; }
        public bool OverrideApplied { get; set; }
    }

    public static class ButtonBackColorExtensions
    {
        // Per-button storage that doesn’t leak and doesn’t use Tag
        private static readonly ConditionalWeakTable<Button, ButtonColorState> _stateTable
            = new ConditionalWeakTable<Button, ButtonColorState>();

        /// <summary>
        /// Set ForeColor and then ensure the BackColor is readable.
        /// Uses original BackColor unless the text is not visible.
        /// </summary>
        public static void SetForeColorWithAutoBack(this Button btn, Color foreColor)
        {
            if (btn == null)
                throw new ArgumentNullException(nameof(btn));

            btn.ForeColor = foreColor;
            btn.EnsureReadableBackColor();
        }

        /// <summary>
        /// Uses the original BackColor unless the text is not visible.
        /// Only then overrides BackColor with a high-contrast color.
        /// </summary>
        public static void EnsureReadableBackColor(this Button btn)
        {
            if (btn == null)
                throw new ArgumentNullException(nameof(btn));

            ButtonColorState state = GetOrCreateState(btn);
            Color originalBack = state.OriginalBackColor;
            Color fore = btn.ForeColor;

            // If text is readable on the original background:
            if (IsReadable(fore, originalBack))
            {
                // If we previously overrode, revert once; otherwise, leave it alone.
                if (state.OverrideApplied)
                {
                    RestoreOriginalBackInternal(btn, state);
                    state.OverrideApplied = false;
                }

                // Important: do NOT set BackColor if we never changed it.
                return;
            }

            // Text is not readable on the original background → override
            Color newBack = GetHighContrastBackColor(fore);

            if (!state.OverrideApplied ||
                btn.BackColor != newBack ||
                btn.UseVisualStyleBackColor)
            {
                btn.UseVisualStyleBackColor = false;
                btn.BackColor = newBack;
                state.OverrideApplied = true;
            }
        }

        /// <summary>
        /// Explicitly restores the original background (and clears any override).
        /// </summary>
        public static void RestoreOriginalBackColor(this Button btn)
        {
            if (btn == null)
            {
                throw new ArgumentNullException(nameof(btn));
            }

            ButtonColorState state = GetOrCreateState(btn);
            RestoreOriginalBackInternal(btn, state);
            state.OverrideApplied = false;
        }

        // ================== INTERNAL HELPERS ==================

        private static ButtonColorState GetOrCreateState(Button btn)
        {
            return _stateTable.GetValue(btn, b =>
            {
                // This runs only once per button, capturing its true original state.
                return new ButtonColorState
                {
                    OriginalBackColor = b.BackColor,
                    OriginalUseVisualStyleBackColor = b.UseVisualStyleBackColor,
                    OverrideApplied = false
                };
            });
        }

        private static void RestoreOriginalBackInternal(Button btn, ButtonColorState state)
        {
            btn.UseVisualStyleBackColor = state.OriginalUseVisualStyleBackColor;

            if (state.OriginalUseVisualStyleBackColor)
            {
                // For themed/default background, let WinForms + OS draw it.
                btn.BackColor = SystemColors.ButtonFace;
                btn.UseVisualStyleBackColor = true;
                //btn.ResetBackColor();
            }
            else
            {
                // Custom original BackColor → restore explicitly.
                btn.BackColor = state.OriginalBackColor;
            }
        }

        /// <summary>
        /// "Readable enough" check.
        /// Conservative: prefers to keep original background unless it's really bad.
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

            // Fallback to contrast ratio (relaxed threshold)
            double ratio = ContrastRatio(fore, back);
            return ratio >= 2.5; // "visibly OK", not strict WCAG
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
