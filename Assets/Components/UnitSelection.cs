using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class UnitSelection : MonoBehaviour {

  // Whether the player is currently selecting troops
  bool isSelecting = false;
  bool disabled = false;

  // The position the current selection box began at
  Vector3 initialMousePos;

  public GameObject selectionCirclePrefab;
  public Color rectColor = new Color( 0.8f, 0.8f, 0.95f, 0.25f );
  public Color rectBorderColor = new Color( 0.8f, 0.8f, 0.95f );

  void Disable() {
    disabled = true;
  }

  void Enable() {
    disabled = false;
  }

  void Update() {

    if (disabled) {
      return;
    }

    // If we press the left mouse button, begin selection and remember
    // initial mouse location
    if (Input.GetMouseButtonDown(0)) {

      isSelecting = true;
      initialMousePos = Input.mousePosition;

      foreach (var selected in FindObjectsOfType<Selectable>()) {
        if (selected.selectionCircle != null) {
          Destroy(selected.selectionCircle.gameObject);
          selected.selectionCircle = null;
        }
      }

    }

    // If player lets go of the left mouse button, end selection
    if (Input.GetMouseButtonUp(0)) {

      // Create a list of the selected objects
      var selectedObjects = new List<Selectable>();
      foreach (var selected in FindObjectsOfType<Selectable>()) {
        if (IsWithinSelectionBounds(selected.gameObject)) {
          selectedObjects.Add(selected);
        }
      }

      // This is just debug output
      var sb = new StringBuilder();
      //sb.AppendLine(string.Format("Selected [{0}] Objects", selectedObjects.Count));
      foreach (var selected in selectedObjects) {
        sb.AppendLine("->" + selected.gameObject.name);
        Debug.Log(sb.ToString());
      }

      GameManager gameManager = Utils.GameManager();
      gameManager.selected = selectedObjects;

      // No longer selecting
      isSelecting = false;

    }

    // Highlight all objects within the selection box
    if (isSelecting) {

      foreach (var selected in FindObjectsOfType<Selectable>()) {
        if (IsWithinSelectionBounds(selected.gameObject)) {
          if (selected.selectionCircle == null) {
            selected.selectionCircle = Instantiate(selectionCirclePrefab);
            selected.selectionCircle.transform.SetParent(selected.transform, false);
          }
        }
        else {
          if (selected.selectionCircle != null) {
            Destroy(selected.selectionCircle.gameObject);
            selected.selectionCircle = null;
          }
        }
      }

    }

  }

  public bool IsWithinSelectionBounds(GameObject gameObject) {
    if (!isSelecting) {
      return false;
    }

    var camera = Camera.main;
    var viewportBounds = Utils.GetViewportBounds(
      camera, initialMousePos, Input.mousePosition
    );

    return viewportBounds.Contains(
      camera.WorldToViewportPoint(gameObject.transform.position)
    );
  }

  void OnGUI() {
    if (isSelecting) {
      var rect = Utils.GetScreenRect(initialMousePos, Input.mousePosition);
      Utils.DrawScreenRect(rect, rectColor);
      Utils.DrawScreenRectBorder(rect, 2, rectBorderColor);
    }
  }

}
