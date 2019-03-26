﻿import { Button, Divider, Dropdown, Menu, Modal, Popconfirm, Row } from 'antd';
import { DialogForm, Visible } from 'components/dataView';
import { DesError } from 'components/decorators';
import lodash from 'lodash';
import { observer } from 'mobx-react';
import * as React from 'react';
import { onAuthorizeActions } from 'store/system/authorize';
import Store from '../store';
import { InfoForm, InsertForm, UpdateForm } from './forms';
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
                <Visible visible={onAuthorizeActions(Store, "delete")}>
                    <Divider type="vertical" />
                    <Button icon="delete" onClick={ActionEvents.onDeleteList} disabled={disabled}> 删除  </Button>
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
                <Visible visible={onAuthorizeActions(Store, "details")}>
                    <DialogForm
                        title="详情"
                        showSubmit={false}
                        type="a"
                    >
                        <InfoForm loadData={data} />
                    </DialogForm>
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