package ru.panovaks.ebay_pulse;

import javafx.application.Application;
import javafx.stage.Stage;
import javafx.scene.Scene;
import javafx.fxml.FXMLLoader;
import javafx.scene.layout.BorderPane;

public class Main extends Application {

    public static void main(String[] args) {
        System.out.println("eBay Pulse");
        launch(Main.class, args);
    }

    @Override
    public void start(Stage stage) throws Exception {
        rootLayout = FXMLLoader.load(getClass().getResource("MainWindow.fxml"));
        Scene scene = new Scene(rootLayout);
        stage.setScene(scene);
        stage.centerOnScreen();
        stage.show();
    }

    private BorderPane rootLayout;
}
