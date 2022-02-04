using System;
using System.Collections.Generic;
using System.Linq;
using UnityX.Geometry;

namespace UnityEngine.UI.Extensions {
    [AddComponentMenu("UI/Extensions/Primitives/UI Polygon")]
    [RequireComponent(typeof(CanvasRenderer))]
    public class UIPolygon : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter {
        [SerializeField]
        Texture _texture;

        [SerializeField]
		Polygon _polygon = new Polygon(new Vector2(0,0), new Vector2(100,0), new Vector2(100,100), new Vector2(0,100));
		public Polygon polygon {
			get {
				return _polygon;
			}
			set {
				_polygon = value;
                if(!_polygon.GetIsClockwise()) {
                    _polygon.FlipWindingOrder();
                }
				SetAllDirty();
			}
		}

        public UVMode uvMode;
        public enum UVMode {
            Rect,
            Shape
        }

        public float uvXAngle = 0;
        public float uvYAngle = 90;

        public float antiAliasing = 2;

        public override Texture mainTexture {
            get {
                return _texture == null ? s_WhiteTexture : _texture;
            }
        }
        public Texture texture {
            get {
                return _texture;
            } set {
                if (_texture == value) return;
                _texture = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        // Caches
        static List<int> triangulatorOutput = new List<int>();
        List<UIVertex[]> segments;

        protected override void Awake() {
            base.Awake();
            useLegacyMeshGeneration = false;
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            SetAllDirty();
        }

        public void SetBoundsFromPoly () {
            var polyRect = polygon.GetRect();

            var localRect = rectTransform.rect;
            var currentRect = new Rect(localRect.x+transform.localPosition.x, localRect.y+transform.localPosition.y, localRect.width, localRect.height);
            var targetRect = new Rect(polyRect.x+currentRect.x, polyRect.y+currentRect.y, polyRect.width, polyRect.height);

            polygon.Move(-polyRect.TopLeft());
            rectTransform.sizeDelta = polyRect.size;
            rectTransform.localPosition = new Vector3(targetRect.center.x, targetRect.center.y, 0);
            
            #if UNITY_EDITOR
            if(!Application.isPlaying && UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this)) {
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(rectTransform);
            }
            #endif
        }

        public void SetPolyToFillRect () {
            polygon.SetVertices(new Rect(0,0,rectTransform.rect.width,rectTransform.rect.height).GetVertices());
            #if UNITY_EDITOR
            if(!Application.isPlaying && UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this)) {
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(rectTransform);
            }
            #endif
        }
        
        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();
            triangulatorOutput.Clear();
			var points = polygon.vertices;
            var pivotOffset = (Vector3)GetPixelAdjustedRect().position;
            Triangulator.GenerateIndices(points, triangulatorOutput);
            var rect = polygon.GetRect();
            Vector2 uvXDirection = MathX.DegreesToVector2(uvXAngle);
            Vector2 uvYDirection = MathX.DegreesToVector2(uvYAngle);
            var distanceXMin = Mathf.Infinity;
            var distanceXMax = Mathf.NegativeInfinity;
            var distanceYMin = Mathf.Infinity;
            var distanceYMax = Mathf.NegativeInfinity;
            foreach(var vert in points) {
                var distanceX = Vector2.Dot(vert, uvXDirection);
                distanceXMin = Mathf.Min(distanceX, distanceXMin);
                distanceXMax = Mathf.Max(distanceX, distanceXMax);
                
                var distanceY = Vector2.Dot(vert, uvYDirection);
                distanceYMin = Mathf.Min(distanceY, distanceYMin);
                distanceYMax = Mathf.Max(distanceY, distanceYMax);
            }

            // using (vh) {
                var color32 = (Color32)color;
                foreach(var point in points) {
                    var pos = new Vector3(point.x, point.y, 0);
                    Vector2 uv = Vector2.zero;
                    
                    if(uvMode == UVMode.Rect) {
                        uv = rect.GetNormalizedPositionInsideRect(pos);
                    } else if(uvMode == UVMode.Shape) {
                        
                        var distanceY = Vector2.Dot(pos, uvYDirection);
                        var distanceX = Vector2.Dot(pos, uvXDirection);
                        var x = Mathf.InverseLerp(distanceXMin, distanceXMax, distanceX);
                        var y = Mathf.InverseLerp(distanceYMin, distanceYMax, distanceY);
                        uv = new Vector2(x,y);
                    }

                    vh.AddVert(pos+pivotOffset, color32, uv);
                }
                // vh.AddUIVertexQuad()
                for(int i = 0; i < triangulatorOutput.Count; i+= 3) {
                    vh.AddTriangle(triangulatorOutput[i], triangulatorOutput[i+1], triangulatorOutput[i+2]);
                }
            // }

            if(antiAliasing > 0) {
                if(segments == null) segments = new List<UIVertex[]>(points.Length);
                else if(segments.Count > points.Length) segments.RemoveRange(points.Length, segments.Count - points.Length);
                while(segments.Count < points.Length) segments.Add(new UIVertex[4]);
                
                float currentUVY = 0;

                void CreateLineSegment(UIVertex[] segment, AdvancedUILineRendererPoint start, AdvancedUILineRendererPoint end, Vector2 pivotOffset, Color32 color, ref float uvY) {
                    AdvancedUILineRenderer.LineSegmentProperties lineSegmentProperties = new AdvancedUILineRenderer.LineSegmentProperties();
                    lineSegmentProperties.start = start;
                    lineSegmentProperties.end = end;
                    lineSegmentProperties.pivotOffset = pivotOffset;
                    lineSegmentProperties.lineAnchor = 0f;
                    lineSegmentProperties.color = color;
                    lineSegmentProperties.innerAlpha = 1;
                    lineSegmentProperties.outerAlpha = 0;
                    lineSegmentProperties.colorBlendMode = ColorX.BlendMode.Normal;
                    lineSegmentProperties.colorBlendWeight = 0;
                    AdvancedUILineRenderer.CreateLineSegment(segment, lineSegmentProperties, ref uvY);
                }

                // (joinAsLoop ? 0 : 1)
                for (var i = 0; i < points.Length; i++) {
                    var startIndex = i;
                    var endIndex = i + 1;
                    if(endIndex >= points.Length) endIndex = 0;
                    CreateLineSegment(segments[i], new AdvancedUILineRendererPoint(points[startIndex], antiAliasing, color), new AdvancedUILineRendererPoint(points[endIndex], antiAliasing, color), pivotOffset, color32, ref currentUVY);
                }
                AdvancedUILineRenderer.Apply(vh, segments, true, AdvancedUILineRenderer.JoinType.Miter);
                // UILine.Build(vh, points, new StrokeGeometryAttributes(0, antiAliasing, Cap.Butt, Join.Bevel, 0, true), pivotOffset, color, color.WithAlpha(0));
            }
		}

		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local);
			Rect rect = GetPixelAdjustedRect();
            // Convert to have lower left corner as reference point.
            local.x += rectTransform.pivot.x * rect.width;
            local.y += rectTransform.pivot.y * rect.height;

            return polygon.ContainsPoint(local);
        }

        


        #region ILayoutElement Interface

        public virtual void CalculateLayoutInputHorizontal() { }
        public virtual void CalculateLayoutInputVertical() { }

        public virtual float minWidth { get { return 0; } }

        public virtual float preferredWidth
        {
            get
            {
                return polygon.GetRect().width;
            }
        }

        public virtual float flexibleWidth { get { return -1; } }

        public virtual float minHeight { get { return 0; } }
        
        public virtual float preferredHeight
        {
            get
            {
                return polygon.GetRect().height;
            }
        }

        public virtual float flexibleHeight { get { return -1; } }

        public virtual int layoutPriority { get { return 0; } }

        #endregion
    }
}