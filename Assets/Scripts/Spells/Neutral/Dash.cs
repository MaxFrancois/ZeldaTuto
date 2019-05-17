//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Dash : Spell
//{
//    public DashConfig Config;

//    private bool CanMove(Transform source, Vector3 direction)
//    {
//        //can't find the right collider for map ?
//        return Physics2D.Raycast(source.position, direction, Config.Distance).collider == null;
//    }

//    public override void Cast(Transform source, Vector3 direction)
//    {
//        if (CanMove(source, direction))
//        {
//            //StartCoroutine(DashCo(source, direction));
//            var currentPosition = source.position;
//            var instance = Instantiate(Config.DashInstance, currentPosition, Quaternion.identity);
//            instance.transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(direction));
//            instance.transform.localScale = new Vector3(Config.Distance / Config.DashWidth, 1f, 1f);
//            source.position += direction * Config.Distance;
//            Destroy(instance, 0.1f);
//            Destroy(this.gameObject, 0.1f);
//        }
//        //else
//        //{
//        //    DestroyThis();
//        //}
//    }

//    //private void DestroyThis()
//    //{
//    //    if (!isDestroyed)
//    //    {
//    //        isDestroyed = true;
//    //        Destroy(this.gameObject, 0.5f);
//    //    }
//    //}

//    //private IEnumerator DashCo(Transform source, Vector3 direction)
//    //{
        
//    //    yield return null;
//    //}
//    public override SpellConfig GetConfig()
//    {
//        return Config;
//    }
//}
