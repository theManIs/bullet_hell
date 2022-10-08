using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyCoxswain))]
public class EnemyCoxswainEditor : Editor
{
	public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!target.GetComponent<Rigidbody2D>() || !target.GetComponent<Collider2D>())
        {
            if (GUILayout.Button("Add Components"))
            {

                if (!target.GetComponent<Rigidbody2D>())
                {
                    Rigidbody2D rb2d = target.AddComponent<Rigidbody2D>();
                    rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                }


                if (!target.GetComponent<Collider2D>())
                {
                    BoxCollider2D bc2d = target.AddComponent<BoxCollider2D>();
                    bc2d.size = new Vector2(bc2d.size.x * .5f, bc2d.size.y * .5f);
                }
            }
        }
        
        if (!target.GetComponent<SplitToPixels>() || !target.GetComponent<DamageAdder>() || !target.GetComponent<SpriteRendererEffectAdder>())
        {
            if (GUILayout.Button("Add Scripts"))
            {
                if (!target.GetComponent<DamageAdder>())
                {
                    target.AddComponent<DamageAdder>();
                }

                if (!target.GetComponent<SplitToPixels>())
                {
                    target.AddComponent<SplitToPixels>();
                }

                if (!target.GetComponent<SpriteRendererEffectAdder>())
                {
                    target.AddComponent<SpriteRendererEffectAdder>();
                }
            }
        }

        if (!target.GetComponent<EnemyCoxswain>().target)
        {
            if (GUILayout.Button("Add Target"))
            {
                target.GetComponent<EnemyCoxswain>().target = GameObject.FindWithTag("Player");
            }
        }
    }
}
