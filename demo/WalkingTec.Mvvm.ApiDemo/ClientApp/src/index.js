/**
 * @author 冷 (https://github.com/LengYXin)
 * @email lengyingxin8966@gmail.com
 * @create date 2018-07-24 02:18:59
 * @modify date 2018-09-10 02:18:59
 * @desc [description]
 * 
 *  react-scripts 有 index.js 入口文件检查机制，所以使用 .js 作为入口 不使用 .ts
 */
import "@babel/polyfill";
import 'antd/dist/antd.less';
import 'viewerjs/dist/viewer.css';
import 'braft-editor/dist/index.css';
import App from "app/index";
import 'nprogress/nprogress.css';
import * as React from 'react';
import ReactDOM from 'react-dom';
import './global.config';
ReactDOM.render(<App />,
  document.getElementById('root'));
