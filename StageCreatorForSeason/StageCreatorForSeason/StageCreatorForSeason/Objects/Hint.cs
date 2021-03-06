﻿using Microsoft.Xna.Framework;
using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Objects
{
    class Hint : Object
    {

        private Object leftTop;
        private Object rightBottom;
        private static Dictionary<eObjectType, string> nameList = new Dictionary<eObjectType, string> {
            { eObjectType.F,"F" },
            { eObjectType.X,"X" },
            { eObjectType.Z,"Z" },
            { eObjectType.Jump,"Jump" },
            { eObjectType.Right,"Right" },
            { eObjectType.Left,"Left" },
            { eObjectType.HintNoLeft,"ChildStopL" },
            { eObjectType.HintNoRight,"ChildStopR" },
            { eObjectType.HintJump,"JumpDown" },
            { eObjectType.HintDown,"ChildJump" },
        };


        public Hint(Vector2 position, eObjectType type)
            : base("", position, 40, type, 0)
        {
            name = nameList[type];
            leftTop = new Object("Point", position - Vector2.One * 60, 30, type, teamNo);
            rightBottom = new Object("Point", position + Vector2.One * 60, 30, type, teamNo);
        }

        public Object GetLeftTop() { return leftTop; }
        public Object GetRightBottom() { return rightBottom; }

        public static eObjectType NameToType(string name) {
            foreach (var n in nameList) {
                if (n.Value == name) { return n.Key; }
            }
            return eObjectType.None;
        }

        public void SetLeft(Vector2 position) { leftTop.Position = position; }
        public void SetRight(Vector2 position) { rightBottom.Position = position; }

        public override string Print() {
            Vector2 center = (rightBottom.Position + leftTop.Position) / 2;
            Vector2 size = rightBottom.Position - leftTop.Position;

            return 
                name + "," +
                (int)position.X + "," + (int)position.Y + " ," +
                (int)center.X + "," + (int)center.Y + " ," +
                (int)size.X + "," + (int)size.Y;
        }

        protected override void DrawObject()
        {
            base.DrawObject();

            Renderer_2D.Begin(Camera2D.GetTransform());

            //Renderer_2D.DrawLine(leftTop.Position, rightBottom.Position, Color.LightBlue, false);

            Vector2 centre = (leftTop.Position + rightBottom.Position) / 2;
            Vector2 square = rightBottom.Position - leftTop.Position;            
            Vector2 size = ResouceManager.GetTextureSize("UnitLine");
            square.X /= size.X;
            square.Y /= size.Y;
            Rectangle rect = new Rectangle(0, 0, (int)size.X, (int)size.Y);

            Renderer_2D.DrawTexture("UnitLine", centre, 0.2f, rect, square, 0, size / 2);

            Renderer_2D.End();
        }


        protected override void DrawCollition()
        {
            Renderer_2D.Begin(Camera2D.GetTransform());

            Color color = isMouseIn ? Color.Red : Color.Yellow;

            if (isMouseIn) {
                leftTop.IsMouseIn = true;
                rightBottom.IsMouseIn = true;
            }

            Vector2 imgSize = ResouceManager.GetTextureSize("CollisionArea");
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Vector2 drawSize = new Vector2(radius / imgSize.X, radius / imgSize.Y) * 2;
            Renderer_2D.DrawTexture("CollisionArea", position, color, 0.5f, rect, drawSize, 0, imgSize / 2);

            Renderer_2D.End();
        }

    }
}
