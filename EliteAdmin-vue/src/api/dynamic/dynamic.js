import request from '@/utils/request'
import { downFile } from '@/utils/request'

/**
* 按表名分页查询
* @param {查询条件} data
*/
export function listDynamicData(data) {
  return request({
    url: 'dynamic/data/list',
    method: 'post',
    data: data,
  })
}

/**
* 按sql编号分页查询
* @param {查询条件} data
*/
export function listDynamicDataBySqlCode(data) {
  return request({
    url: 'dynamic/data/lilistBySqlCodest',
    method: 'post',
    data: data,
  })
}

/**
* 按表名查询字段配置信息
* @param {查询条件} data
*/
export function getFiledConfigInfo(tableName, userId) {
  return request({
    url: `dynamic/data/getFiledConfigInfo?tableName=${tableName}&userId=${userId}`,
    method: 'get',
  })
}

/**
* 按表名查询字段配置信息中包含的数组
* @param {查询条件} data
*/
export function getkeyvalueGroup(query) {
  return request({
    url: 'dynamic/data/getkeyvalueGroup?tableName=' + query,
    method: 'get',
  })
}


/**
* 新增字段信息配置
* @param data
*/
export function addDynamicObject(data) {
  return request({
    url: 'dynamic/data/insert',
    method: 'post',
    data: data,
  })
}
/**
* 修改字段信息配置
* @param data
*/
export function updateBaseFieldConfig(data) {
  return request({
    url: 'elite/BaseFieldConfig',
    method: 'PUT',
    data: data,
  })
}
/**
* 获取字段信息配置详情
* @param {Id}
*/
export function getBaseFieldConfig(id) {
  return request({
    url: 'elite/BaseFieldConfig/' + id,
    method: 'get'
  })
}

/**
* 删除字段信息配置
* @param {主键} pid
*/
export function delBaseFieldConfig(pid) {
  return request({
    url: 'elite/BaseFieldConfig/' + pid,
    method: 'delete'
  })
}
// 导出字段信息配置
export async function exportBaseFieldConfig(query) {
  await downFile('elite/BaseFieldConfig/export', { ...query })
}
