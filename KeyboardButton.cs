using System.Drawing;
using JsonObjectConverter;
using System;

namespace Keys_Visualizer
{
    public class KeyboardButton
    {
        public static int DEFAULT_SIZE { get; } = 60;
        public string name { get; private set; }
        public string text { get; private set; }
        public Point? location { get; private set; }
        public Size? size { get; private set; }
        public Color? fontColor { get; private set; }
        public Color? backColor { get; private set; }
        public Color? backColorOnActive { get; private set; }

        public KeyboardButton(string name) : this(name, name, null, null, null, null, null)
        {

        }
        public KeyboardButton(string name, string text, Point? location, Size? size, Color? fontColor, Color? backColor, Color? backColorOnActive)
        {
            this.name = name;
            this.text = text;
            if (location.HasValue)
            {
                this.location = location.Value;
            }
            if (size.HasValue)
            {
                this.size = size.Value;
            }
            if (fontColor.HasValue)
            {
                this.fontColor = fontColor.Value;
            }
            if (backColor.HasValue)
            {
                this.backColor = backColor.Value;
            }
            if (backColor.HasValue)
            {
                this.backColorOnActive = backColorOnActive.Value;
            }
        }

        public KeyboardButton(Json json)
        {
            if (json.tupleList != null && json.tupleList != null)
            {
                setValues(json);
            }
        }

        public string generateJson()
        {
            return generateJson(0);
        }
        public string generateJson(short tabsCount)
        {
            return Json.generateJsonFromObject(this, tabsCount);
        }
        private void setValues(Json helper)
        {
            if (helper.tupleList != null)
            {
                short jsonArraysCounter = 0;
                foreach (Tuple<string, Tuple<string, bool>> tuple in helper.tupleList)
                {
                    string temp = tuple.Item2.Item1.Trim(' ').Trim('\"');
                    if (!temp.ToLower().Equals("null"))
                    {
                        string item1 = tuple.Item1.Trim(' ').Trim('\"');
                        var property = this.GetType().GetProperty(item1);
                        if (property != null && temp != string.Empty)
                        {
                            object valueToSet;
                            if (item1.StartsWith("[") || item1.StartsWith("{"))
                            {
                                valueToSet = Json.convertStringToList(item1, property.PropertyType);
                                property.SetValue(this, valueToSet);
                            }
                            else
                            {
                                if (property.Name.ToLower().Equals("location")|| property.Name.ToLower().Equals("size"))
                                {
                                    Json tempJson = new Json(tuple.Item2.Item1, tuple.Item1);
                                    int x = Convert.ToInt32(tempJson.items[0].Split('=')[1]);
                                    int y = Convert.ToInt32(tempJson.items[1].Split('=')[1]);
                                    if (property.Name.ToLower().Equals("location"))
                                    {
                                        this.location = new Point(x, y);
                                    }
                                    else
                                    {
                                        this.size = new Size(x, y);
                                    }
                                    jsonArraysCounter++;
                                }
                                else if (property.Name.ToLower().Contains("color"))
                                {
                                    Color tempColor = getColorByString(tuple.Item2.Item1);
                                    switch (property.Name)
                                    {
                                        case "backColor": this.backColor = tempColor; break;
                                        case "fontColor": this.fontColor = tempColor; break;
                                        case "backColorOnActive": this.backColorOnActive = tempColor; break;
                                    }
                                }
                                else
                                {
                                    valueToSet = Json.convertStringToType(temp, property.PropertyType);
                                    property.SetValue(this, valueToSet);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static Color getColorByString(string color)
        {
            string colorString = color.Replace("Color [", string.Empty).TrimEnd(']');
            if (colorString.Contains(","))
            {
                string[] splitted = colorString.Split(',');
                return Color.FromArgb(Convert.ToInt32(splitted[1].Split('=')[1]), Convert.ToInt32(splitted[2].Split('=')[1]), Convert.ToInt32(splitted[3].Split('=')[1]));
            }
            else
            {
                return Color.FromName(colorString);
            }
        }
    }
}
