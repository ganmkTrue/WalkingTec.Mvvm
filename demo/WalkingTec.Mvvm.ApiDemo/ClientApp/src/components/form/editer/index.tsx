/**
 * @author 冷 (https://github.com/LengYXin)
 * @email lengyingxin8966@gmail.com
 * @create date 2018-09-12 18:53:42
 * @modify date 2018-09-12 18:53:42
 * @desc [description]
*/
import { Card } from 'antd';
import BraftEditor, { BraftEditorProps, EditorState } from 'braft-editor';
import 'braft-editor/dist/index.css';
import lodash from 'lodash';
import React from 'react';
import './style.less';



interface IAppProps extends BraftEditorProps {
  display?: boolean;
  disabled?: boolean;
  [key: string]: any;
}
/**
 * https://github.com/margox/braft-editor
 * 富文本编辑
 */
export class WtmEditor extends React.Component<IAppProps, any> {
  static wtmType = "Editor";
  console = true
  default: BraftEditorProps = {

  }
  componentDidMount() {
    // console.log("componentDidMount", this.props)

  }
  state = {
    editorState: BraftEditor.createEditorState(lodash.unescape(this.props.value)), // 设置编辑器初始内容
  }
  onChange(editorState: EditorState) {
    if (lodash.isEqual(this.state.editorState.toRAW(), editorState.toRAW())) {
      return this.setState({ editorState });
    }
    this.setState({ editorState });
    const value = editorState.toHTML()
    this.console && console.log(value)
    this.props.onChange && this.props.onChange(lodash.escape(lodash.eq(value, '<p></p>') ? '' : value))
  }
  render() {
    const { editorState } = this.state;
    const props = { ...this.default, ...this.props }
    delete props.value;
    delete props.onChange;
    // 禁用
    if (this.props.display) {
      props.controls = [];
      props.disabled = true;
    }
    return <Card className="app-editor-card">
      <BraftEditor
        value={editorState}
        onChange={this.onChange.bind(this)}
        {...props}
      />
    </Card>
  }
}
export default WtmEditor

