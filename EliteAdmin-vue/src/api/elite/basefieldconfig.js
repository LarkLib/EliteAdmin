import request from '@/utils/request'
import { downFile } from '@/utils/request'

/**
* 字段信息配置分页查询
* @param {查询条件} data
*/
export function listBaseFieldConfig(query) {
  return request({
    url: 'elite/BaseFieldConfig/list',
    method: 'get',
    params: query,
  })
}

/**
* 新增字段信息配置
* @param data
*/
export function addBaseFieldConfig(data) {
  return request({
    url: 'elite/BaseFieldConfig',
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
