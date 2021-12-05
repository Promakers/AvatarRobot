using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public abstract class ListAnnotation<T> : HierarchicalAnnotation where T : HierarchicalAnnotation {
    [SerializeField] GameObject annotationPrefab;

    [SerializeField] List<T> _children;
    protected List<T> children {
      get {
        if (_children == null) {
          _children = new List<T>();
        }
        return _children;
      }
    }

    public T this[int index] {
      get { return children[index]; }
    }

    public int count {
      get { return children.Count; }
    }

    public void Fill(int count) {
      // TODO: YK node list
      int idx = 0;
      while (children.Count < count) {
        children.Add(InstantiateChild(false, idx++));
      }
    }

    public void Add(T element) {
      children.Add(element);
    }

    public override bool isMirrored {
      set {
        foreach (var child in children) {
          child.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle {
      set {
        foreach (var child in children) {
          child.rotationAngle = value;
        }
        base.rotationAngle = value;
      }
    }

    protected virtual void Destroy() {
      foreach (var child in children) {
        Destroy(child);
      }
      _children = null;
    }

    protected virtual T InstantiateChild(bool isActive = true, int idx =0) {
      var annotation = base.InstantiateChild<T>(annotationPrefab, idx);
      annotation.SetActive(isActive);
      //annotation.name = $"{annotation.name}_{idx}";
      return annotation;
    }

    /// <summary>
    ///   Zip <see cref="children" /> and <paramref name="argumentList" />, and call <paramref name="action" /> with each pair.
    ///   If <paramref name="argumentList" /> has more elements than <see cref="children" />, <see cref="children" /> elements will be initialized with <see cref="InstantiateChild" />.
    /// </summary>
    /// <param name="action">
    ///   This will receive 2 arguments and return void.
    ///   The 1st argument is <typeparamref name="T" />, that is an ith element in <see cref="children" />.
    ///   The 2nd argument is <typeparamref name="S" />, that is also an ith element in <paramref name="argumentList" />.
    /// </param>
    protected void CallActionForAll<S>(IList<S> argumentList, Action<T, S> action) {
      for (var i = 0; i < Mathf.Max(children.Count, argumentList.Count); i++) {
        if (i >= argumentList.Count) {
          // children.Count > argumentList.Count
          action(children[i], default(S));
          continue;
        }
        // TODO: YK connection list
        // reset annotations
        if (i >= children.Count) {
          // children.Count < argumentList.Count
          children.Add(InstantiateChild(true, i));
        } else if (children[i] == null) {
          // child is not initialized yet
          children[i] = InstantiateChild(true, i);
        }
        action(children[i], argumentList[i]);
      }
    }
  }
}
