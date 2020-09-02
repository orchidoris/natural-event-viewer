const path = require('path');
const webpack = require('webpack');
const TsconfigPathsPlugin = require('tsconfig-paths-webpack-plugin');

module.exports = {
    entry: path.join(__dirname, './../boot-client.tsx'),
    resolve: {
        plugins: [
            new TsconfigPathsPlugin({
                configFile: path.join(__dirname, './../../tsconfig.json'),
            }),
        ],
        extensions: ['.js', '.jsx', '.ts', '.tsx', 'css', 'less'],
    },
    output: {
        path: path.join(__dirname, './../../../NaturalEventsViewer.Web/dist'),
        publicPath: 'dist/',
        filename: 'main-client.js',
    },
    module: {
        rules: [
            {
                exclude: /\.test\.tsx?$/,
                test: /\.tsx?$/,
                loader: 'ts-loader',
            },
            {
                test: /\.(png|jpg|jpeg|gif|svg|ttf)$/,
                use: 'url-loader?limit=25000',
            },
            {
                test: /\.(css|less)(\?|$)/,
                include: path.join(__dirname, './../'),
                use: [
                    'style-loader',
                    'css-modules-typescript-loader',
                    {
                        loader: 'css-loader',
                        options: {
                            modules: true,
                            camelCase: true,
                            // minimize: false,
                            localIdentName: '[name]__[local]',
                        },
                    },
                    'less-loader',
                ],
            },
            {
                test: /DateRangePicker\.css$/,
                include: path.join(__dirname, './../../node_modules/@wojtekmaj/react-daterange-picker'),
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /Calendar\.css$/,
                include: path.join(__dirname, './../../node_modules/react-calendar'),
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /react-table\.css$/,
                include: path.join(__dirname, './../../node_modules/react-table'),
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /styles\.css$/,
                include: path.join(__dirname, './../../node_modules/react-table-hoc-fixed-columns/lib'),
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /style\.css$/,
                include: path.join(__dirname, './../../node_modules/react-vis/dist'),
                use: ['style-loader', 'css-loader'],
            },
        ],
    },
    plugins: [
        new webpack.WatchIgnorePlugin([/(css|less)\.d\.ts$/])
    ],
};
