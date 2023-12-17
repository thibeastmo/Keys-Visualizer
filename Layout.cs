using System;
using System.Collections.Generic;
using System.Text;
using JsonObjectConverter;

namespace Keys_Visualizer
{
    public class Layout
    {
        public string name { get; }
        public List<KeyboardButton> keyboardButtons { get; }

        public Layout(string name)
        {
            this.name = name;
        }

        public Layout(string name, List<KeyboardButton> keyboardButtons)
        {
            this.name = name;
            this.keyboardButtons = keyboardButtons;
        }

        public Layout(Json json)
        {
            this.name = json.tupleList[0].Item2.Item1;
            if (json.jsonArray != null && json.jsonArray.Count > 0)
            {
                if (keyboardButtons == null)
                {
                    keyboardButtons = new List<KeyboardButton>();
                }
                foreach (Tuple<string, Json> kb in json.jsonArray)
                {
                    this.keyboardButtons.Add(new KeyboardButton(kb.Item2));
                }
            }
        }

        public string generateJson()
        {
            return generateJson(0);
        }
        public string generateJson(short tabsCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Json.generateTabs(tabsCount, 0) + "{");
            sb.Append(Json.generateTabs(tabsCount, 1));
            sb.AppendLine("\"name\": \"" + this.name + "\",");
            sb.Append(Json.generateTabs(tabsCount, 1));
            sb.Append("\"keyboardButtons\": ");
            if (this.keyboardButtons != null && this.keyboardButtons.Count > 0)
            {
                sb.AppendLine(" [");
                bool firstTime = true;
                foreach (KeyboardButton kb in this.keyboardButtons)
                {
                    if (firstTime)
                    {
                        firstTime = false;
                    }
                    else
                    {
                        sb.AppendLine(",");
                    }
                    sb.Append(kb.generateJson((short)(tabsCount + 2)));
                }
                sb.AppendLine("\n" + Json.generateTabs(tabsCount, 1) + "]");
            }
            else
            {
                sb.AppendLine("null");
            }
            sb.Append(Json.generateTabs(tabsCount, 0));
            sb.Append("}");
            return sb.ToString();
        }
    }
}
