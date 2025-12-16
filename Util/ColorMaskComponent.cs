using System;
using Code.Entities;
using UnityEngine;

namespace Code.Util
{
    public class ColorMaskComponent : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Renderer meshRenderer;

        private readonly int _colorHash = Shader.PropertyToID("_Color");
        private Entity _entity;
        private Material _mat;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _mat = meshRenderer.material;
        }

        public void SetColor(Color color)
        {
            _mat.SetColor(_colorHash, color);
        }
    }
}