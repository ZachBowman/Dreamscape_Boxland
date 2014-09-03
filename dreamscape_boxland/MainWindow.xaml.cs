using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dreamscape_boxland
  {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged
    {
    const int map_tile_max_width = 55;   // max width of map
    const int map_tile_max_length = 25;  // max length of map
    const int map_tile_max_height = 4;   // max height of map

    int map_tile_width = 0;   // actual width of map
    int map_tile_length = 0;  // actual length of map
    int map_tile_height = 0;  // actual height of map

    string[,] textmap = new string[map_tile_max_length, map_tile_max_height];
    
    int current_layer = 0;         // which vertical layer the user's working in
    string current_tile = "";      // which tile the user is painting with
    ImageSource current_tilefile;  // the source file for the current tile

    public event PropertyChangedEventHandler PropertyChanged;

    public int bound_layer
      {
      get
        {
        return this.current_layer;
        }
      set
        {
        this.current_layer = value;
        this.PropertyChanged (this, new PropertyChangedEventArgs ("bound_layer"));
        }
      }

    public MainWindow ()
      {
      this.DataContext = this;

      InitializeComponent ();

      // fill the grid with empty spaces
      for (int z = 0; z < map_tile_max_height; z += 1)
        {
        for (int y = 0; y < map_tile_max_length; y += 1)
          {
          for (int x = 0; x < map_tile_max_width; x += 1)
            {
            textmap[y, z] = String.Concat (textmap[y, z], ".. ");
            }
          }
        }
      
      test_info.Text = textmap[0, 0];

      //layer_textbox.Text = "Layer " + Convert.ToString (current_layer);
      }

    ////////////////////////////////////////////////////////////////////////////////

    // move up one layer
    private void up_layer_button_Click (object sender, RoutedEventArgs e)
      {
      if (bound_layer < map_tile_max_height - 1) bound_layer += 1;
      //layer_textbox.Text = "Layer " + Convert.ToString (current_layer);
      }

    ////////////////////////////////////////////////////////////////////////////////

    // move down one layer
    private void down_layer_button_Click (object sender, RoutedEventArgs e)
      {
      if (bound_layer > 0) bound_layer -= 1;
      //layer_textbox.Text = "Layer " + Convert.ToString (current_layer);
      }

    ////////////////////////////////////////////////////////////////////////////////

    // click on tile 1
    private void texture_click (object sender, MouseButtonEventArgs e)
      {
      var image = sender as Image;
      current_tilefile = image.Source;
      string source_string = Convert.ToString (image.Source);
      int last_slash = source_string.LastIndexOf ('/');
      current_tile = source_string.Substring (last_slash + 1, 3);

      // move green border to selected tile
      tile_border.Visibility = Visibility.Visible;
      Canvas.SetLeft (tile_border, Canvas.GetLeft (image) - 2);
      Canvas.SetTop  (tile_border, Canvas.GetTop  (image) - 2);
      }

    ////////////////////////////////////////////////////////////////////////////////

    // click on canvas
    private void editor_canvas_MouseDown (object sender, MouseButtonEventArgs e)
      {
      textmap[0, current_layer] = substring_replace (textmap[0, current_layer], 0, 3, current_tile);

      //test_info.Text = current_tile;
      test_info.Text = textmap[0, current_layer];

      Point mouse = Mouse.GetPosition (editor_canvas);
      if (current_tilefile != null) draw_sprite (editor_canvas, current_tilefile, Convert.ToInt32 (mouse.X), Convert.ToInt32 (mouse.Y));//100, 100);
      }

    ////////////////////////////////////////////////////////////////////////////////

    // draw a bitmap on a canvas from a file name
    public void draw_sprite (Canvas canvas, ImageSource image_file, int x, int y)
      {
      Image image = new Image
        {
        Source = new BitmapImage (new Uri (image_file.ToString ())),
        };

      canvas.Children.Add (image);
      Canvas.SetLeft (image, x);
      Canvas.SetTop  (image, y);
      }

    ////////////////////////////////////////////////////////////////////////////////

    // replace a substring
    public string substring_replace (string str, int start, int length, string sub)
      {
      var builder = new StringBuilder (str);
      builder.Remove (start, length);
      builder.Insert (start, sub);
      str = builder.ToString ();

      return str;
      }
    }
  }
