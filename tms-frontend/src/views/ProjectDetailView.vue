<template>
  <div class="project-detail-container">
    <!-- 頁首 -->
    <div class="header">
      <div class="header-left">
        <el-button @click="router.back()"> ← 返回 </el-button>

        <div>
          <h2 class="title">
            {{ project?.name || '專案' }}
          </h2>

          <div class="subtitle">Project #{{ projectId }}</div>
        </div>
      </div>

      <div class="header-actions">
        <el-button type="primary" @click="showCreateDialog = true"> 新增測試案例 </el-button>
      </div>
    </div>

    <!-- Project 資訊 -->
    <el-card class="detail-card">
      <el-descriptions :column="2" border>
        <el-descriptions-item label="狀態">
          {{ project?.status }}
        </el-descriptions-item>

        <el-descriptions-item label="Owner">
          {{ project?.ownerName || '-' }}
        </el-descriptions-item>

        <el-descriptions-item label="描述" :span="2">
          {{ project?.description || '無描述' }}
        </el-descriptions-item>
      </el-descriptions>
    </el-card>

    <!-- TestCase 區塊 -->
    <div class="section-header">
      <div class="section-title">
        <h3>測試案例列表</h3>

        <span class="testcase-count"> 共 {{ testCases.length }} 筆 </span>
      </div>

      <el-button
        type="danger"
        :disabled="selectedTestCases.length === 0"
        :loading="deletingTestCases"
        @click="deleteSelectedTestCases"
      >
        刪除已選取
        <span v-if="selectedTestCases.length > 0"> （{{ selectedTestCases.length }}） </span>
      </el-button>
    </div>

    <el-card>
      <el-table
        :data="testCases"
        style="width: 100%"
        empty-text="目前沒有測試案例"
        @selection-change="handleTestCaseSelectionChange"
      >
        <el-table-column type="selection" width="55" />

        <el-table-column prop="title" label="標題" />

        <el-table-column prop="priority" label="優先級" width="110" />

        <el-table-column prop="status" label="狀態" width="110" />

        <el-table-column label="操作" width="100">
          <template #default="scope">
            <el-button size="small" @click="goToTestCase(scope.row.id)"> 查看 </el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- 新增 TestCase -->
    <el-dialog v-model="showCreateDialog" title="新增測試案例" width="400px">
      <el-form :model="newTestCase">
        <el-form-item label="標題">
          <el-input v-model="newTestCase.title" />
        </el-form-item>

        <el-form-item label="描述">
          <el-input v-model="newTestCase.description" type="textarea" />
        </el-form-item>

        <el-form-item label="優先級">
          <el-select v-model="newTestCase.priority" style="width: 100%">
            <el-option label="High" value="High" />

            <el-option label="Medium" value="Medium" />

            <el-option label="Low" value="Low" />
          </el-select>
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="showCreateDialog = false"> 取消 </el-button>

        <el-button type="primary" @click="createTestCase"> 確認 </el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import api from '../api'

const router = useRouter()
const route = useRoute()

const projectId = route.params.id

const project = ref(null)
const testCases = ref([])

const showCreateDialog = ref(false)

const selectedTestCases = ref([])
const deletingTestCases = ref(false)

const newTestCase = ref({
  title: '',
  description: '',
  priority: 'Medium',
})

const fetchProject = async () => {
  try {
    const response = await api.get(`/api/project/${projectId}`)

    project.value = response.data
  } catch (error) {
    console.error(error)

    ElMessage.error('取得專案資料失敗')
  }
}

const fetchTestCases = async () => {
  try {
    const response = await api.get(`/api/testcase/project/${projectId}`)

    testCases.value = response.data
  } catch (error) {
    console.error(error)

    ElMessage.error('取得測試案例失敗')
  }
}

const createTestCase = async () => {
  try {
    await api.post('/api/testcase', {
      projectId: parseInt(projectId),
      title: newTestCase.value.title,
      description: newTestCase.value.description,
      priority: newTestCase.value.priority,
    })

    ElMessage.success('測試案例建立成功')

    showCreateDialog.value = false

    newTestCase.value = {
      title: '',
      description: '',
      priority: 'Medium',
    }

    await fetchTestCases()
  } catch (error) {
    console.error(error)

    ElMessage.error('建立測試案例失敗')
  }
}

const goToTestCase = (id) => {
  router.push(`/testcases/${id}`)
}

const handleTestCaseSelectionChange = (rows) => {
  selectedTestCases.value = rows
}

const deleteSelectedTestCases = async () => {
  if (selectedTestCases.value.length === 0) {
    return
  }

  try {
    await ElMessageBox.confirm(
      `確定要刪除選取的 ${selectedTestCases.value.length} 筆測試案例嗎？`,
      '刪除測試案例',
      {
        confirmButtonText: '刪除',
        cancelButtonText: '取消',
        type: 'warning',
      },
    )

    deletingTestCases.value = true

    let successCount = 0
    let failedCount = 0

    for (const testCase of selectedTestCases.value) {
      try {
        await api.delete(`/api/testcase/${testCase.id}`)

        successCount++
      } catch (error) {
        console.error(`刪除 TestCase ${testCase.id} 失敗`, error)

        failedCount++
      }
    }

    if (failedCount === 0) {
      ElMessage.success(`成功刪除 ${successCount} 筆測試案例`)
    } else {
      ElMessage.warning(`成功 ${successCount} 筆，失敗 ${failedCount} 筆`)
    }

    selectedTestCases.value = []

    await fetchTestCases()
  } catch (error) {
    if (error === 'cancel' || error === 'close') {
      return
    }

    console.error(error)

    ElMessage.error('刪除測試案例時發生錯誤')
  } finally {
    deletingTestCases.value = false
  }
}

onMounted(() => {
  fetchProject()
  fetchTestCases()
})
</script>

<style scoped>
.project-detail-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 24px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 20px;
  margin-bottom: 24px;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 16px;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 8px;
}

.title {
  margin: 0;
  font-size: 24px;
  line-height: 1.3;
}

.subtitle {
  margin-top: 4px;
  font-size: 13px;
  color: #909399;
}

.detail-card {
  margin-bottom: 28px;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 12px;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 12px;
}

.section-title h3 {
  margin: 0;
}

.testcase-count {
  font-size: 14px;
  color: #909399;
}

@media (max-width: 768px) {
  .header {
    align-items: flex-start;
    flex-direction: column;
  }

  .header-actions {
    width: 100%;
  }

  .section-header {
    align-items: flex-start;
    flex-direction: column;
    gap: 12px;
  }
}
</style>
