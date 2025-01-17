﻿using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;
public class OptionMenuButton : MonoBehaviour {

  public enum selected_btn
  {
    layout, exit, up, down
  }
  private AudioSource asource;
  public GameObject layout_game_button;
  public GameObject exit_game_button;
  public GameObject up_game_button;
  public GameObject down_game_button;
  public GameObject time_text_holder;
  private InputDevice inputDevice;

  int min_time = 0;
  int max_time = 100;
  console_button layout_btn;
  console_button exit_btn;
  console_button up_btn;
  console_button down_btn;

  public GameObject main_menu_canvas;


  public GameObject layout_a_obj;
  public GameObject layout_b_obj;
  public selected_btn curr_selected;
  // Use this for initialization
  void Start()
  {
    layout_a_obj.SetActive(true);
    layout_b_obj.SetActive(false);
    asource = this.GetComponent<AudioSource>();
    inputDevice = (InputManager.Devices.Count > 0) ? InputManager.Devices[0] : null;
    //game_manager.gstate = vars.game_state.main_menu;
    curr_selected = selected_btn.down;
    layout_btn = layout_game_button.GetComponent<console_button>();
    exit_btn = exit_game_button.GetComponent<console_button>();
    up_btn = up_game_button.GetComponent<console_button>();
    down_btn = down_game_button.GetComponent<console_button>();
    set_btn_state();
    game_manager.game_time = 60;
    time_text_holder.GetComponent<Text>().text = game_manager.volume.ToString() + "";
  }


    public void make_visible()
    {

        set_btn_state();

    }


    // Update is called once per frame
    void Update()
  {
    if (inputDevice != null)
    {
      if (inputDevice.DPadDown.WasPressed || (inputDevice.LeftStick.Down.State && !inputDevice.LeftStick.Down.LastState))
      {
        select_next_btn();
      }

      if (inputDevice.DPadUp.WasPressed || (inputDevice.LeftStick.Up.State && !inputDevice.LeftStick.Up.LastState))
      {
        select_prev_btn();
      }

      if (inputDevice.Action1.WasPressed)
      {
        click_btn();

      }
    }
  }



  public void select_next_btn()
  {

    switch (curr_selected)
    {
      case selected_btn.down:
        curr_selected = selected_btn.up;
        break;
      case selected_btn.up:
        curr_selected = selected_btn.layout;
        break;
      case selected_btn.layout:
        curr_selected = selected_btn.exit;
        break;
      case selected_btn.exit:
        curr_selected = selected_btn.down;
        break;
      default:
        break;
    }
    set_btn_state();
  }


  public void select_prev_btn()
  {
    switch (curr_selected)
    {
      case selected_btn.down:
        curr_selected = selected_btn.exit;
        break;
      case selected_btn.exit:
        curr_selected = selected_btn.layout;
        break;
      case selected_btn.layout:
        curr_selected = selected_btn.up;
        break;
      case selected_btn.up:
        curr_selected = selected_btn.down;
        break;
      default:
        break;
    }
    set_btn_state();
  }
  bool layout_normal = true;
  public void click_btn()
  {
    asource.Play();
    switch (curr_selected)
    {
      case selected_btn.layout:
        layout_btn.set_pressed();
        if (layout_normal)
        {
          GameObject.Find("vars").GetComponent<vars>().set_player_control_preset(vars.player_control_presets.normal_2);
          layout_normal = false;
          layout_a_obj.SetActive(false);
          layout_b_obj.SetActive(true);
        }
        else
        {
          GameObject.Find("vars").GetComponent<vars>().set_player_control_preset(vars.player_control_presets.normal);
          layout_normal = true;
          layout_a_obj.SetActive(true);
          layout_b_obj.SetActive(false);
        }
       
     
          layout_btn.set_selected();
        break;
      case selected_btn.exit:
        game_manager.gstate = vars.game_state.main_menu;
        Application.Quit();
        break;
      case selected_btn.up:
        up_btn.set_pressed();
        if (game_manager.volume <= max_time - 10)
        {
          game_manager.volume += 10;
        }
      
        time_text_holder.GetComponent<Text>().text = game_manager.volume.ToString() + "";
        up_btn.set_selected();
        break;
      case selected_btn.down:
        down_btn.set_pressed();

        if (game_manager.volume >= min_time + 10)
        {
          game_manager.volume -= 10;
        }
        time_text_holder.GetComponent<Text>().text = game_manager.volume.ToString() + "";
        down_btn.set_selected();
        break;
      default:
        break;
    }
  }

  public void set_btn_state()
  {
    switch (curr_selected)
    {
      case selected_btn.layout:
        layout_btn.set_selected();
        exit_btn.set_normal();
        up_btn.set_normal();
        down_btn.set_normal();
        break;
      case selected_btn.exit:
        layout_btn.set_normal();
        exit_btn.set_selected();
        up_btn.set_normal();
        down_btn.set_normal();
        break;
      case selected_btn.up:
        layout_btn.set_normal();
        exit_btn.set_normal();
        up_btn.set_selected();
        down_btn.set_normal();
        break;
      case selected_btn.down:
        layout_btn.set_normal();
        exit_btn.set_normal();
        up_btn.set_normal();
        down_btn.set_selected();
        break;
      default:
        curr_selected = selected_btn.layout;
        set_btn_state();
        break;
    }
  }
}
