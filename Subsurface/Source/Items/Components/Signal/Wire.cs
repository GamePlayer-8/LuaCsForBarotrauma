﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Barotrauma.Items.Components
{
    class Wire : ItemComponent, IDrawableComponent
    {
        class WireSection
        {
            private Vector2 start;

            private float angle;
            private float length;

            public WireSection(Vector2 start, Vector2 end)
            {
                this.start = start;

                angle = MathUtils.VectorToAngle(end - start);
                length = Vector2.Distance(start, end);
            }

            public void Draw(SpriteBatch spriteBatch, Color color, Vector2 offset, float depth, float width = 0.3f)
            {
                spriteBatch.Draw(wireSprite.Texture,
                    new Vector2(start.X+offset.X, -(start.Y+offset.Y)), null, color,
                    -angle,
                    new Vector2(0.0f, wireSprite.size.Y / 2.0f),
                    new Vector2(length / wireSprite.Texture.Width, width),
                    SpriteEffects.None,
                    depth);
            }

            public static void Draw(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float depth, float width = 0.3f)
            {
                start.Y = -start.Y;
                end.Y = -end.Y;

                spriteBatch.Draw(wireSprite.Texture,
                    start, null, color,
                    MathUtils.VectorToAngle(end - start),
                    new Vector2(0.0f, wireSprite.size.Y / 2.0f),
                    new Vector2((Vector2.Distance(start, end)) / wireSprite.Texture.Width, width),
                    SpriteEffects.None,
                    depth);
            }
        }

        const float nodeDistance = 32.0f;
        const float heightFromFloor = 128.0f;

        static Sprite wireSprite;

        public List<Vector2> Nodes;

        private bool sectionsDirty;
        private List<WireSection> sections;

        Connection[] connections;

        private Vector2 newNodePos;

        private static Wire draggingWire;
        private static int? selectedNodeIndex;

        public bool Hidden, Locked;

        public Connection[] Connections
        {
            get { return connections; }
        }
                
        public Wire(Item item, XElement element)
            : base(item, element)
        {
            if (wireSprite == null)
            {
                wireSprite = new Sprite("Content/Items/wireHorizontal.png", new Vector2(0.5f, 0.5f));
                wireSprite.Depth = 0.85f;
            }
            
            Nodes = new List<Vector2>();
            sections = new List<WireSection>();

            connections = new Connection[2];
            
            IsActive = false;
        }
        
        public Connection OtherConnection(Connection connection)
        {
            if (connection == null) return null;
            if (connection == connections[0]) return connections[1];
            if (connection == connections[1]) return connections[0];

            return null;
        }

        public bool IsConnectedTo(Item item)
        {
            if (connections[0] != null && connections[0].Item == item) return true;
            return (connections[1] != null && connections[1].Item == item);
        }

        public void RemoveConnection(Item item)
        {
            for (int i = 0; i<2; i++)
            {
                if (connections[i]==null || connections[i].Item!=item) continue;
                
                for (int n = 0; n< connections[i].Wires.Length; n++)
                {
                    if (connections[i].Wires[n] != this) continue;
                    
                    connections[i].Wires[n] = null;
                    connections[i].UpdateRecipients();
                }
                connections[i] = null;
            }
        }

        public void RemoveConnection(Connection connection)
        {
            if (connection == connections[0]) connections[0] = null;            
            if (connection == connections[1]) connections[1] = null;
        }

        public bool Connect(Connection newConnection, bool addNode = true, bool loading = false)
        {
            for (int i = 0; i < 2; i++)
            {
                if (connections[i] == newConnection) return false;
            }

            if (!connections.Any(c => c == null)) return false;

            for (int i = 0; i < 2; i++)
            {
                if (connections[i] != null && connections[i].Item == newConnection.Item)
                {
                    addNode = false;
                    break;
                }
            }

            if (item.body != null) item.Submarine = newConnection.Item.Submarine;

            for (int i = 0; i < 2; i++)
            {
                if (connections[i] != null) continue;

                connections[i] = newConnection;

                if (!addNode) break;

                if (newConnection.Item.Submarine == null) continue;

                if (Nodes.Count > 0 && Nodes[0] == newConnection.Item.Position - newConnection.Item.Submarine.HiddenSubPosition) break;
                if (Nodes.Count > 1 && Nodes[Nodes.Count-1] == newConnection.Item.Position - newConnection.Item.Submarine.HiddenSubPosition) break;
                               

                if (i == 0)
                {
                    Nodes.Insert(0, newConnection.Item.Position - newConnection.Item.Submarine.HiddenSubPosition);                    
                }
                else
                {
                    Nodes.Add(newConnection.Item.Position - newConnection.Item.Submarine.HiddenSubPosition);
                }

                
                break;
            }

            if (connections[0] != null && connections[1] != null)
            {
                foreach (ItemComponent ic in item.components)
                {
                    if (ic == this) continue;
                    ic.Drop(null);
                }
                if (item.Container != null) item.Container.RemoveContained(this.item);

                if (item.body != null) item.body.Enabled = false;

                IsActive = false;

                CleanNodes();
            }

            Drawable = Nodes.Any();

            if (!loading) Item.NewComponentEvent(this, true, true);

            UpdateSections();

            return true;
        }

        public override void Equip(Character character)
        {
            ClearConnections();

            IsActive = true;
            //Drawable = true;
        }

        public override void Unequip(Character character)
        {
            ClearConnections();

            IsActive = false;
        }

        public override void Drop(Character dropper)
        {
            ClearConnections();
            
            IsActive = false;
        }

        public override void Update(float deltaTime, Camera cam)
        {
            if (Nodes.Count == 0) return;

            Submarine sub = null;
            if (connections[0] != null && connections[0].Item.Submarine != null) sub = connections[0].Item.Submarine;
            if (connections[1] != null && connections[1].Item.Submarine != null) sub = connections[1].Item.Submarine;

            if (item.Submarine != sub && Screen.Selected != GameMain.EditMapScreen)
            {
                ClearConnections();
                Nodes.Clear();
                return;
            }

            newNodePos = RoundNode(item.Position, item.CurrentHull) - sub.HiddenSubPosition;
        }

        public override bool Use(float deltaTime, Character character = null)
        {
            if (character == Character.Controlled && character.SelectedConstruction != null) return false;

            if (newNodePos!= Vector2.Zero && Nodes.Count>0 && Vector2.Distance(newNodePos, Nodes[Nodes.Count - 1]) > nodeDistance)
            {
                Nodes.Add(newNodePos);
                UpdateSections();

                Drawable = true;

                newNodePos = Vector2.Zero;
            }
            return true;
        }

        public override void SecondaryUse(float deltaTime, Character character = null)
        {
            if (Nodes.Count > 1)
            {
                Nodes.RemoveAt(Nodes.Count - 1);
                UpdateSections();

                item.NewComponentEvent(this, true, true);
            }

            Drawable = Nodes.Any();
        }

        public override bool Pick(Character picker)
        {
            ClearConnections();

            return true;
        }

        private void UpdateSections()
        {
            sections.Clear();

            for (int i = 0; i < Nodes.Count-1; i++)
            {
                sections.Add(new WireSection(Nodes[i], Nodes[i + 1]));
            }
        }

        private void ClearConnections()
        {
            Nodes.Clear();
            sections.Clear();

            for (int i = 0; i < 2; i++)
            {
                if (connections[i] == null) continue;
                int wireIndex = connections[i].FindWireIndex(item);

                if (wireIndex == -1) continue;
                connections[i].AddLink(wireIndex, null);

                connections[i] = null;
            }

            Drawable = false;
        }

        private Vector2 RoundNode(Vector2 position, Hull hull)
        {
            if (Screen.Selected == GameMain.EditMapScreen)
            {
                position.X = MathUtils.Round(position.X, Submarine.GridSize.X / 2.0f);
                position.Y = MathUtils.Round(position.Y, Submarine.GridSize.Y / 2.0f);
            }
            else
            {
                position.X = MathUtils.Round(position.X, nodeDistance);
                if (hull == null)
                {
                    position.Y = MathUtils.Round(position.Y, nodeDistance);
                }
                else
                {
                    position.Y -= hull.Rect.Y - hull.Rect.Height;
                    position.Y = Math.Max(MathUtils.Round(position.Y, nodeDistance), heightFromFloor);
                    position.Y += hull.Rect.Y -hull.Rect.Height;
                }
            }

            return position;
        }

        private void CleanNodes()
        {
            for (int i = Nodes.Count - 2; i > 0; i--)
            {
                if ((Nodes[i - 1].X == Nodes[i].X || Nodes[i - 1].Y == Nodes[i].Y) &&
                    (Nodes[i + 1].X == Nodes[i].X || Nodes[i + 1].Y == Nodes[i].Y))
                {
                    if (Vector2.Distance(Nodes[i - 1], Nodes[i]) == Vector2.Distance(Nodes[i + 1], Nodes[i]))
                    {
                        Nodes.RemoveAt(i);
                    }
                }
            }

            bool removed;
            do
            {
                removed = false;
                for (int i = Nodes.Count - 2; i > 0; i--)
                {
                    if ((Nodes[i - 1].X == Nodes[i].X && Nodes[i + 1].X == Nodes[i].X)
                        || (Nodes[i - 1].Y == Nodes[i].Y && Nodes[i + 1].Y == Nodes[i].Y))
                    {
                        Nodes.RemoveAt(i);
                        removed = true;
                    }
                }

            } while (removed);

        }

        public void Draw(SpriteBatch spriteBatch, bool editing)
        {
            if (!Nodes.Any())
            {
                Drawable = false;
                return;
            }

            Vector2 drawOffset = Vector2.Zero;
            if (item.Submarine != null)
            {
                drawOffset = item.Submarine.DrawPosition + item.Submarine.HiddenSubPosition;
            }

            float depth = wireSprite.Depth + ((item.ID % 100) * 0.00001f);

            if (item.IsHighlighted)
            {
                foreach (WireSection section in sections)
                {
                    section.Draw(spriteBatch, Color.Gold, drawOffset, depth, 0.5f);
                }
            }

            foreach (WireSection section in sections)
            {
                section.Draw(spriteBatch, item.Color, drawOffset, depth, 0.3f);
            }
            
            if (IsActive && Vector2.Distance(newNodePos, Nodes[Nodes.Count - 1]) > nodeDistance)
            {
                WireSection.Draw(
                    spriteBatch,
                    new Vector2(Nodes[Nodes.Count - 1].X, Nodes[Nodes.Count - 1].Y) + drawOffset, 
                    new Vector2(newNodePos.X, newNodePos.Y) + drawOffset, 
                    item.Color * 0.5f,
                    depth, 
                    0.3f);
            }

            if (!editing || !PlayerInput.MouseInsideWindow || !GameMain.EditMapScreen.WiringMode) return;
            if (Character.Controlled != null && Character.Controlled.SelectedConstruction != null) return;

            for (int i = 0; i < Nodes.Count; i++)
            {
                Vector2 worldPos = Nodes[i];
                if (item.Submarine != null) worldPos += item.Submarine.Position + item.Submarine.HiddenSubPosition;
                worldPos.Y = -worldPos.Y;

                GUI.DrawRectangle(spriteBatch, worldPos + new Vector2(-3, -3), new Vector2(6, 6), item.Color, true, 0.0f);

                if (IsActive) continue;

                if (GUIComponent.MouseOn != null ||
                    Vector2.Distance(GameMain.EditMapScreen.Cam.ScreenToWorld(PlayerInput.MousePosition), new Vector2(worldPos.X, -worldPos.Y)) > 10.0f)
                {
                    continue;
                }

                GUI.DrawRectangle(spriteBatch, worldPos + new Vector2(-10, -10), new Vector2(20, 20), Color.Red, false, 0.0f);

                if (selectedNodeIndex == null && draggingWire == null)// && !MapEntity.SelectedAny)
                {
                    if (PlayerInput.LeftButtonDown())
                    {
                        MapEntity.DisableSelect = true;
                        MapEntity.SelectEntity(item);
                        draggingWire = this;
                        selectedNodeIndex = i;
                        break;
                    }
                    else if (PlayerInput.RightButtonClicked())
                    {
                        Nodes.RemoveAt(i);
                        break;
                    }
                }
            }

            if (PlayerInput.LeftButtonHeld())
            {
                if (selectedNodeIndex != null && draggingWire == this)
                {
                    MapEntity.DisableSelect = true;
                    //Nodes[(int)selectedNodeIndex] = GameMain.EditMapScreen.Cam.ScreenToWorld(PlayerInput.MousePosition)-Submarine.HiddenSubPosition+Submarine.Loaded.Position;


                    Submarine sub = null;
                    if (connections[0] != null && connections[0].Item.Submarine != null) sub = connections[0].Item.Submarine;
                    if (connections[1] != null && connections[1].Item.Submarine != null) sub = connections[1].Item.Submarine;

                    Vector2 nodeWorldPos = GameMain.EditMapScreen.Cam.ScreenToWorld(PlayerInput.MousePosition) - sub.HiddenSubPosition - sub.Position;// Nodes[(int)selectedNodeIndex];

                    nodeWorldPos.X = MathUtils.Round(nodeWorldPos.X, Submarine.GridSize.X / 2.0f);
                    nodeWorldPos.Y = MathUtils.Round(nodeWorldPos.Y, Submarine.GridSize.Y / 2.0f);

                    //if (item.Submarine != null) nodeWorldPos += item.Submarine.Position;

                    Nodes[(int)selectedNodeIndex] = nodeWorldPos;

                    MapEntity.SelectEntity(item);
                }
            }
            else
            {
                selectedNodeIndex = null;
                draggingWire = null;
            }
        }

        public override void FlipX()
        {            
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i] = new Vector2(-Nodes[i].X, Nodes[i].Y);
            }            
        }

        public override XElement Save(XElement parentElement)
        {
            XElement componentElement = base.Save(parentElement);

            if (Nodes == null || Nodes.Count == 0) return componentElement;

            string[] nodeCoords = new string[Nodes.Count * 2];
            for (int i = 0; i < Nodes.Count; i++)
            {
                nodeCoords[i * 2] = Nodes[i].X.ToString(CultureInfo.InvariantCulture);
                nodeCoords[i * 2 + 1] = Nodes[i].Y.ToString(CultureInfo.InvariantCulture);
            }

            componentElement.Add(new XAttribute("nodes", string.Join(";", nodeCoords)));

            return componentElement;
        }

        public override void Load(XElement componentElement)
        {
            base.Load(componentElement);

            string nodeString = ToolBox.GetAttributeString(componentElement, "nodes", "");
            if (nodeString == "") return;

            string[] nodeCoords = nodeString.Split(';');
            for (int i = 0; i < nodeCoords.Length / 2; i++)
            {
                float x = 0.0f, y = 0.0f;

                try
                {
                    x = float.Parse(nodeCoords[i * 2], CultureInfo.InvariantCulture);
                }
                catch { x = 0.0f; }

                try
                {
                    y = float.Parse(nodeCoords[i * 2 + 1], CultureInfo.InvariantCulture);
                }
                catch { y = 0.0f; }

                Nodes.Add(new Vector2(x, y));
            }

            Drawable = Nodes.Any();

        }

        protected override void RemoveComponentSpecific()
        {
            ClearConnections();

            base.RemoveComponentSpecific();
        }

        public override bool FillNetworkData(Networking.NetworkEventType type, Lidgren.Network.NetBuffer message)
        {
            message.Write((byte)Math.Min(Nodes.Count, 10));
            for (int i = 0; i < Math.Min(Nodes.Count,10); i++)
            {
                message.Write(Nodes[i].X);
                message.Write(Nodes[i].Y);
            }

            return true;
        }

        public override void ReadNetworkData(Networking.NetworkEventType type, Lidgren.Network.NetIncomingMessage message, float sendingTime)
        {
            Nodes.Clear();

            List<Vector2> newNodes = new List<Vector2>();
            int nodeCount = message.ReadByte();
            for (int i = 0; i<nodeCount; i++)
            {
                Vector2 newNode = new Vector2(message.ReadFloat(), message.ReadFloat());
                if (!MathUtils.IsValid(newNode)) return;
                newNodes.Add(newNode);
            }

            Nodes = newNodes;

            Drawable = Nodes.Any();
        }
    }
}
