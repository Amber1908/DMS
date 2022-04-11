"use strict";

var path = require("path");
var WebpackNotifierPlugin = require("webpack-notifier");
var BrowserSyncPlugin = require("browser-sync-webpack-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = env => {
    var publicPath = "/QuestCreator/Scripts/dist/";

    if (env === "test") {
        publicPath = "/TestDMS/Scripts/dist/";
    }

    if (env === "production") {
        publicPath = "/DMS/Scripts/dist/";
    }

    if (env === "debug") {
        publicPath = "/DebugDMS/Scripts/dist/";
    }

    if (env === "dmdms") {
        publicPath = "/DMDMS/Scripts/dist/";
    }

    return {
        entry: {
            Index: "./Scripts/Components/Root.js",
            PDFViewer: "./Scripts/Components/PDFViewer/Root.js"
        },
        output: {
            path: path.resolve(__dirname, "./Scripts/dist"),
            filename: "[name].js",
            publicPath: publicPath
        },
        module: {
            rules: [
                {
                    test: /\.js$/,
                    exclude: /node_modules/,
                    use: {
                        loader: "babel-loader"
                    }
                },
                {
                    test: /\.css$/i,
                    use: ['style-loader', 'css-loader'],
                },
            ]
        },
        devtool: "source-map",
        plugins: [
            new CleanWebpackPlugin(),
        ]
    }
};