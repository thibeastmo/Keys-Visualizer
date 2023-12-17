using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Keys_Visualizer
{
    public partial class ButtonSettingsForm : Form
    {
        private bool hasToBeRemoved = false;
        public ButtonSettingsForm(KeyboardButton keyboardButton)
        {
            InitializeComponent();
            txtName.Text = keyboardButton.name;
            txtText.Text = keyboardButton.text;
            if (keyboardButton.location.HasValue)
            {
                txtLocationX.Text = keyboardButton.location.Value.X.ToString();
                txtLocationY.Text = keyboardButton.location.Value.Y.ToString();
            }
            if (keyboardButton.size.HasValue)
            {
                txtSizeHeight.Text = keyboardButton.size.Value.Height.ToString();
                txtSizeWidth.Text = keyboardButton.size.Value.Width.ToString();
            }
            if (keyboardButton.fontColor.HasValue)
            {
                cdFont.Color = keyboardButton.fontColor.Value;
                btnFontColor.BackColor = cdFont.Color;
            }
            if (keyboardButton.backColor.HasValue)
            {
                cdBackColor.Color = keyboardButton.backColor.Value;
                btnBackColor.BackColor = cdBackColor.Color;
            }
            if (keyboardButton.backColorOnActive.HasValue)
            {
                cdOnActiveColor.Color = keyboardButton.backColorOnActive.Value;
                btnOnActiveColor.BackColor = cdOnActiveColor.Color;
            }
        }

        private void ButtonSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.lastEditedKeyboardButton = new Tuple<bool, KeyboardButton>(hasToBeRemoved, initializeKeyboardbutton());
        }

        private static List<char> numbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private void numberTextBox_TextChanged(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char character in (sender as TextBox).Text)
            {
                if (numbers.Contains(character))
                {
                    sb.Append(character);
                }
            }
            (sender as TextBox).Text = sb.ToString();
        }

        private void btnFontColor_Click(object sender, EventArgs e)
        {
            cdFont.ShowDialog();
            (sender as Button).BackColor = cdFont.Color;
        }

        private void btnBackColor_Click(object sender, EventArgs e)
        {
            cdBackColor.ShowDialog();
            (sender as Button).BackColor = cdBackColor.Color;
        }

        private void btnOnActiveColor_Click(object sender, EventArgs e)
        {
            cdOnActiveColor.ShowDialog();
            (sender as Button).BackColor = cdOnActiveColor.Color;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            hasToBeRemoved = true;
            this.Close();
        }

        private KeyboardButton initializeKeyboardbutton()
        {
            Point location;
            if (txtLocationX.Text.Length > 0 && txtLocationY.Text.Length > 0)
            {
                location = new Point(Convert.ToInt32(txtLocationX.Text), Convert.ToInt32(txtLocationY.Text));
            }
            else
            {
                location = new Point();
            }
            Size size;
            if (txtSizeHeight.Text.Length > 0 && txtSizeWidth.Text.Length > 0)
            {
                size = new Size(Convert.ToInt32(txtSizeWidth.Text), Convert.ToInt32(txtSizeHeight.Text));
            }
            else
            {
                size = new Size(KeyboardButton.DEFAULT_SIZE, KeyboardButton.DEFAULT_SIZE);
            }
            return new KeyboardButton(txtName.Text, txtText.Text, location, size, (cdFont.Color != null ? cdFont.Color : Form1.DefaultForeColor), (cdBackColor.Color != null ? cdBackColor.Color : Form1.DefaultBackColor), (cdOnActiveColor.Color != null ? cdOnActiveColor.Color : Form1.defaultButtonColorOnActive));
        }
    }
}
