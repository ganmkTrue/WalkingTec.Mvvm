import { Button, Divider, Dropdown, Menu, message, Modal, Popconfirm, Row } from 'antd';
import { DialogForm, Visible } from 'components/dataView';
import { DesError } from 'components/decorators';
import lodash from 'lodash';
import { observer } from 'mobx-react';
import * as React from 'react';
import { onAuthorizeActions } from 'store/system/authorize';
import Frameworkuserbase from '../../frameworkuserbase';
import Store from '../store';
import { InfoForm, InsertForm, TestForm, UpdateForm } from './forms';
/**
 * 动作事件
 */
export const ActionEvents = {
    onTest(event: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
        event.preventDefault();
        event.stopPropagation();
    },
    /**
     * 导入
     */
    onImport() {
        Store.onPageState("visiblePort", true)
    },
    /**
     * 导出
     */
    onExport() {
        Store.onExport()
    },
    /**
     * 批量导出
     */
    onExportIds() {
        Store.onExportIds([...Store.selectedRowKeys])
    },
    /**
     * 删除
     * @param data 
     */
    onDelete(data) {
        Store.onDelete([lodash.get(data, Store.IdKey)])
    },
    /**
    * 删除
    */
    onDeleteList() {
        const length = Store.selectedRowKeys.length
        Modal.confirm({
            title: `确定删除 ${length} 条数据?`,
            onOk: async () => {
                Store.onDelete([...Store.selectedRowKeys])
            },
            onCancel() { },
        });
    },
}
/**
 * 表格 所有 动作
 */
@DesError
@observer
class PageAction extends React.Component<any, any> {
    render() {
        const { selectedRowKeys } = Store;
        const deletelength = selectedRowKeys.length;
        const disabled = deletelength < 1;
        return (
            <Row className="data-view-page-action">
                <DialogForm
                    title="所有表单控件"
                    icon="plus"
                    button={props => {
                        return <>
                            <Button onClick={props.onSubmit}>提交</Button>
                            <Divider type="vertical" />
                            <Button onClick={props.onCancel}>关闭</Button>
                            <Divider type="vertical" />
                            <Button onClick={() => { message.warn('点了按钮') }}>按钮3</Button>
                            <Divider type="vertical" />
                            <Button>按钮4</Button>
                        </>
                    }}
                // width={500}
                >
                    <TestForm />
                </DialogForm>
                <Divider type="vertical" />

                <Visible visible={onAuthorizeActions(Store, "insert")}>
                    <DialogForm
                        title="新建"
                        icon="plus"
                    // width={500}
                    >
                        <InsertForm />
                    </DialogForm>
                </Visible>
                <Visible visible={onAuthorizeActions(Store, "update")}>
                    <Divider type="vertical" />
                    <DialogForm
                        title="修改"
                        icon="edit"
                        disabled={deletelength != 1}
                    >
                        <UpdateForm loadData={() => (lodash.find(Store.selectedRowKeys))} />
                    </DialogForm>
                </Visible>
                <Visible visible={onAuthorizeActions(Store, "delete")}>
                    <Divider type="vertical" />
                    <Button icon="delete" onClick={ActionEvents.onDeleteList} disabled={disabled}> 删除  </Button>
                </Visible>
                <Visible visible={onAuthorizeActions(Store, "import")}>
                    <Divider type="vertical" />
                    <Button icon="folder-add" onClick={ActionEvents.onImport}>导入</Button>
                </Visible>
                <Visible visible={onAuthorizeActions(Store, "export")}>
                    <Divider type="vertical" />
                    <Dropdown overlay={<Menu>
                        <Menu.Item>
                            <a onClick={ActionEvents.onExport}>导出全部</a>
                        </Menu.Item>
                        <Menu.Item disabled={disabled}>
                            <a onClick={ActionEvents.onExportIds}>导出勾选</a>
                        </Menu.Item>
                    </Menu>}>
                        <Button icon="download" >导出</Button>
                    </Dropdown>
                </Visible>

            </Row>
        );
    }
}
/**
 * 表格 行 动作
 */
@DesError
@observer
class RowAction extends React.Component<{
    /** 数据详情 */
    data: any;
    [key: string]: any;
}, any> {
    render() {
        const { data } = this.props
        return (
            <Row className="data-view-row-action">
                <DialogForm
                    title="测试"
                    showSubmit={false}
                    type="a"
                >
                    <Frameworkuserbase defaultSearchParams={{ ITCode: "测试" }} />
                </DialogForm>
                <Divider type="vertical" />
                <Visible visible={onAuthorizeActions(Store, "details")}>
                    <DialogForm
                        title="详情"
                        showSubmit={false}
                        type="a"
                    >
                        <InfoForm loadData={data} />
                    </DialogForm>
                </Visible>
                <Visible visible={onAuthorizeActions(Store, "update")}>
                    <Divider type="vertical" />
                    <DialogForm
                        title="修改"
                        type="a"
                    >
                        <UpdateForm loadData={data} />
                    </DialogForm>
                </Visible>
                <Visible visible={onAuthorizeActions(Store, "delete")}>
                    <Divider type="vertical" />
                    <Popconfirm title="确定删除?" onConfirm={() => { ActionEvents.onDelete(data) }} >
                        <a >删除</a>
                    </Popconfirm>
                </Visible>
            </Row>
        );
    }
}
export default {
    /**
     * 页面动作
     */
    pageAction: PageAction,
    /**
     * 数据行动作
     */
    rowAction: RowAction
}